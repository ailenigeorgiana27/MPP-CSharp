using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Model;
using Networking.DTO;
using Services;

namespace Networking.JsonProtocol;

public class ClientWorker: IObserver
{
    private IServices server;
    private TcpClient connection;

    private NetworkStream stream;
    private volatile bool connected;
    
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ClientWorker");
    public ClientWorker(IServices server, TcpClient connection)
    {
        this.server = server;
        this.connection = connection;
        try
        {
            stream=connection.GetStream();
            connected=true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }
    }
    
    public virtual void run()
    {
        using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        while(connected)
        {
            try
            {
                string requestJson = reader.ReadLine(); // Read JSON line-by-line
                if (string.IsNullOrEmpty(requestJson)) continue;
                log.DebugFormat("Received json request {0}",requestJson);
                Console.WriteLine("Received json request {0}",requestJson);
                Request request = JsonSerializer.Deserialize<Request>(requestJson);
                log.DebugFormat("Deserializaed Request {0} ",request);
                Console.WriteLine("Deserialized Request {0} ",request);
                Response response =handleRequest(request);
                if (response!=null)
                {
                    sendResponse(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (e.InnerException!=null)
                    log.ErrorFormat("run inner error {0}",e.InnerException.Message);
                log.Error(e.StackTrace);
            }
				
            try
            {
                Thread.Sleep(1000);
            }
            catch (Exception e)
            {
                log.Error(e.StackTrace);
            }
        }
        try
        {
            stream.Close();
            connection.Close();
        }
        catch (Exception e)
        {
            log.Error("Error "+e);
        }
    }
    private Response handleRequest(Request request)
    { Console.WriteLine("handling request");
        try
        {   
            if (request.Type == RequestType.Login)
            {
                return HandleLogin(request);
            }
            else if (request.Type == RequestType.Logout)
            {
                return HandleLogout(request);
            }
            else if (request.Type == RequestType.Register)
            {
                return HandleRegister(request);
            }
            else if (request.Type == RequestType.Load)
            {
                return HandleLoad(request);
            }
            else if (request.Type == RequestType.Filter)
            {
                return HandleFilter(request);
            }
            
            return new Response(ResponseType.ERROR, "Unknown request");
        }
        catch (Exception e)
        {
            return new Response(ResponseType.ERROR, e.Message);
        }
    }

    private void sendResponse(Response response)
    {
        String jsonString=JsonSerializer.Serialize(response);
        log.DebugFormat("sending response {0}",jsonString);
        lock (stream)
        {
            byte[] data = Encoding.UTF8.GetBytes(jsonString + "\n"); 
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }

    }

    private Response HandleLogin(Request request)
    {Console.WriteLine("handle login");
        try
        {
            UserDTO dto = JsonSerializer.Deserialize<UserDTO>(request.Data.ToString());
            server.login(dto.Username, dto.Password, this);
            return new Response(ResponseType.OK, null);
        }
        catch (Exception e)
        {
            return new Response(ResponseType.ERROR, e.Message);
        }
    }


    private Response HandleLogout(Request request)
    {
        try
        {
            UserDTO dto = JsonSerializer.Deserialize<UserDTO>(request.Data.ToString());
            var username = dto.Username;
            Console.WriteLine(username + "Logout request");
            server.logout(username, this);
            connected = false;
            return new Response(ResponseType.OK, null);
        }
        catch (Exception e)
        {
            return new Response(ResponseType.ERROR, e.Message);
        }
    }

    private Response HandleRegister(Request request)
    {
        try
        {
            InscriereDTO dto = JsonSerializer.Deserialize<InscriereDTO>(request.Data.ToString());
            server.inscriereParticipant(new Participant(
                0, dto.name, dto.age), dto.probaIds);
            return new Response(ResponseType.OK, null);

        }
        catch (Exception e)
        {
            return new Response(ResponseType.ERROR, e.Message);
        }
        
    }

    private Response HandleLoad(Request request)
    {
        try
        {
            List<ProbaDTO> data = new List<ProbaDTO>();
            foreach (var entry in server.getAllProbe())
            {
                var probaDTO = new ProbaDTO(entry.Key.Id, entry.Key.Distanta,
                    entry.Key.Stil, entry.Value);
                data.Add(probaDTO);
            }
        
            return new Response(ResponseType.OK, data);
        }
        catch (Exception e)
        {
            return new Response(ResponseType.ERROR, e.Message);
        }
    }


    private Response HandleFilter(Request request)
    {
        try
        {
            ProbaDTO dto = JsonSerializer.Deserialize<ProbaDTO>(request.Data.ToString());


            Dictionary<Participant, Int32> result = server.getParticipantsByProba(dto.Distanta, dto.Stil);
            List<ParticipantDTO> data = new List<ParticipantDTO>();
            foreach (var resultKey in result.Keys)
            {
                var entity = new ParticipantDTO(resultKey.Id, resultKey.Age,
                    resultKey.Name, result[resultKey]);
                data.Add(entity);
            }

            return new Response(ResponseType.OK, data);
        }
        catch (Exception e)
        {
            return new Response(ResponseType.ERROR, null);
            
        }

}

    public void Update(Participant participant, Int64[] probaIds)
    { Console.WriteLine("worker: participant added");
        try
        {
            InscriereDTO dto = new InscriereDTO(
                participant.Id, participant.Age,
                participant.Name, probaIds);
            sendResponse(new Response(ResponseType.Participant_Inscris, dto));

        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }
        
        
    }
}