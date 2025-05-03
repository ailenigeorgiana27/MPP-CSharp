using Model;
using Networking.DTO;
using Services;

namespace Networking.JsonProtocol;

using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using log4net;

public class Proxy: IServices
{   private string host;
    private int port;

    private IObserver client;
    private NetworkStream stream;
    private TcpClient connection;
    private Queue<Response> responses;
    private volatile bool finished;
    private EventWaitHandle _waitHandle;
    private static readonly ILog log = LogManager.GetLogger(typeof(Proxy));
    
    public Proxy(string host, int port)
    {
        this.host = host;
        this.port = port;
        responses=new Queue<Response>();
    }
    
    
    private void initializeConnection()
    {
        try
        {
            connection=new TcpClient(host,port);
            stream=connection.GetStream();
            finished=false;
            _waitHandle = new AutoResetEvent(false);
            startReader();
        }catch (Exception e){
            Console.WriteLine(e.StackTrace);
        }
    }
    private void startReader()
    {
        Thread tw =new Thread(run);
        tw.Start();
    }
    public void login(string username, string password, IObserver client)
    {   log.InfoFormat("proxy: loggin attempt for {0} {1}",username,password);
        initializeConnection();
        Console.WriteLine("Proxy: login attempt for {0} {1}",username,password);
        string encodedPassword = password;
        this.client = client;
        Request request = new Request(RequestType.Login, new UserDTO(1,username, encodedPassword));
        
        sendRequest(request);

        Response response = readResponse();
        if (response.Type == ResponseType.ERROR) {
            closeConnection();
            throw new Exception(response.Data.ToString());
            
        }
    }

    public void logout(String username, IObserver client)
    {
        Request request = new Request(RequestType.Logout, new UserDTO(0, username, null));
        sendRequest(request);
        Response response = readResponse();
        closeConnection();
        if (response.Type == ResponseType.ERROR) 
        {
            throw new Exception(response.Data.ToString());
        }
    }

    public Dictionary<Proba, int> getAllProbe()
    {
        Request request = new Request(RequestType.Load, null);
        sendRequest(request);
        Response response = readResponse();
        if (response.Type == ResponseType.ERROR)
        {
            throw new Exception(response.Data.ToString());
        }

        Dictionary<Proba, int> probes = new Dictionary<Proba, int>();
        foreach (var item in (JsonSerializer.Deserialize<List<ProbaDTO>>(response.Data.ToString())))
        {
            Proba proba = new Proba( item.ID,item.Distanta, item.Stil);
            probes.Add(proba, item.Participants);
        }
        return probes;
    }
    public void inscriereParticipant(Participant participant, long[] probaIds)
    {
            Request request = new Request(RequestType.Register, new InscriereDTO(participant.Id, 
                participant.Age,participant.Name, probaIds));
            sendRequest(request);
            Response response = readResponse();
            if (response.Type == ResponseType.ERROR) 
            {
                throw new Exception(response.Data.ToString());
            }
        
    }

    public Dictionary<Participant, int> getParticipantsByProba(int distance, string style)
    {
        Request request = new Request(RequestType.Filter, new ProbaDTO(1,distance, style, 0));
        sendRequest(request);
        Response response = readResponse();
        if (response.Type == ResponseType.ERROR) 
        {
            throw new Exception(response.Data.ToString());
        }
        Dictionary<Participant, int> result = new Dictionary<Participant, int>();
        foreach (var item in (JsonSerializer.Deserialize<List<ParticipantDTO>>(response.Data.ToString())))
        {
            result.Add(new Participant( item.id, item.name, item.age), item.noProbe);
        }
        return result;
    }
    
    
    
    private void closeConnection()
    {
        finished=true;
        try
        {
            stream.Close();
            connection.Close();
            _waitHandle.Close();
            client=null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }
    }

    private void sendRequest(Request request)
    {
        try
        {
            lock (stream)
            {
					
                string jsonRequest = JsonSerializer.Serialize(request);
                Console.WriteLine("made json request "+jsonRequest);
                log.DebugFormat("Sending request {0}",jsonRequest);
                byte[] data = Encoding.UTF8.GetBytes(jsonRequest + "\n"); 
                stream.Write(data, 0, data.Length);
                Console.WriteLine("sent request "+jsonRequest);
                stream.Flush();
					
            }
        }
        catch (Exception e)
        {
            throw new Exception("Error sending object "+e);
        }

    }

    private Response readResponse()
    {
        Response response =null;
        try
        {
            _waitHandle.WaitOne();
            lock (responses)
            {
                response = responses.Dequeue();
                
            }
        }catch (Exception e) {
            Console.WriteLine(e.StackTrace);
        }
        return response;
    }
    
    
    public virtual void run()
    {
        using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        while(!finished)
        {
            try
            {
                string responseJson = reader.ReadLine();
                if (string.IsNullOrEmpty(responseJson)) 
                    continue; 
                Response response=JsonSerializer.Deserialize<Response>(responseJson);
                log.Debug("response received "+response);
                if (response.Type == ResponseType.Participant_Inscris)
                {   Console.WriteLine("response received "+response);
                    InscriereDTO data = JsonSerializer.Deserialize<InscriereDTO>(response.Data.ToString());
                    client.Update(new Participant( data.id,data.name, data.age), data.probaIds);
                }
                else
                {
                    lock (responses)
                    {
                        responses.Enqueue(response);
                    }
                    _waitHandle.Set();
                }
            }
            catch (Exception e)
            {
                log.Error("Reading error "+e);
            }
					
        }
    }
    
}