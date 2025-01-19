using CrypticLauncherBeautify.Core;
using log4net;
using Newtonsoft.Json.Linq;
using WebSocketSharp;

public class WebSocketManager
{
    private static readonly ILog Log = LogManager.GetLogger(typeof(WebSocketManager));
    public static WebSocket? WebSocket;
    
    private static async Task SendWebSocketRequestAsync(JObject request)
    {
        try
        {
            if (WebSocket != null && WebSocket.IsAlive)
            {
                WebSocket.Send(request.ToString());
                Task.Delay(50).Wait();
                Log.Info("Request sent: " + request["method"]?.ToString());
            }
            else
            {
                Log.Error("WebSocket is not connected.");
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error sending request: {ex.Message}", ex);
        }
    }
    
    private static JObject CreateEvaluateRequest(string expression, int id = 2)
    {
        return new JObject
        {
            { "id", id },
            { "method", "Runtime.evaluate" },
            { "params", new JObject
                {
                    { "expression", expression }
                }
            }
        };
    }
    
    private static JObject CreateCallFunctionRequest(string objectId, string functionDeclaration, int id = 2)
    {
        return new JObject
        {
            { "id", id },
            { "method", "Runtime.callFunctionOn" },
            { "params", new JObject
                {
                    { "objectId", objectId },
                    { "functionDeclaration", "function() { return document.location.href; }" }
                }
            }
        };
    }
    
    public static async Task ChangeBackgroundAsync(string backgroundUrl)
    {
        string expression;
        if (GlobalVariables.IsLoginPage)
        {
            expression = $"document.querySelector('section[style*=\"background-image: url(\\'/static/img/sto/bg-login.jpg\\')\"]').style.backgroundImage = \"url('{backgroundUrl}')\";";
        }
        else
        {
            expression = $"document.querySelector('section[style*=\"background-image: url(\\'/static/img/sto/bg-patching.jpg\\')\"]').style.backgroundImage = \"url('{backgroundUrl}')\";";
        }
        var request = CreateEvaluateRequest(expression);
        await SendWebSocketRequestAsync(request);
    }
    
    public static async Task ChangeCssHrefAsync(string newCssHref)
    {
        string expression = $"document.querySelector('link[rel=\"stylesheet\"][href=\"/static/css/sto.css?v3.4\"]').href = '{newCssHref}';\n;";
        var request = CreateEvaluateRequest(expression);
        await SendWebSocketRequestAsync(request);
    }
    
    public static async Task GetReadyState()
    {
        string expression = "document.readyState;";
        var request = CreateEvaluateRequest(expression);
        await SendWebSocketRequestAsync(request);
    }
    
    public static async Task GetHrefAsync(string objectId)
    {
        string expression = $"function() {{ return document.location.href; }}";
        var request = CreateCallFunctionRequest(objectId, expression);
        await SendWebSocketRequestAsync(request);
    }
    
    public static async Task ChangeSubmitContentAsync(string content)
    {
        string expression;
        if (GlobalVariables.IsLoginPage)
        {
            expression = $"document.querySelectorAll('input').forEach(input => {{ if (input.type === 'submit' && input.classList.contains('disabled') && input.value === 'Login') input.value = '{content}'; }});\n";
        }
        else
        {
            expression = $"document.querySelectorAll('input').forEach(input => {{ if (input.type === 'submit' && input.classList.contains('action') && input.value === 'Engage') input.value = '{content}'; }});\n";
        }
        
        var request = CreateEvaluateRequest(expression);
        await SendWebSocketRequestAsync(request);
    }
    
    public static async Task ChangeArcIcon(string newIcon)
    {
        string expression = $"document.querySelector('img[alt=\"Arc Games\"]').src = '{newIcon}';" + $"document.querySelector('img[alt=\"Arc Games\"]').style.height = '20px';\n";
        var request = CreateEvaluateRequest(expression);
        await SendWebSocketRequestAsync(request);
    }
    
    public static async Task ChangeServerNameAsync(string holodeckStr, string tribbleStr)
    {
        string expression = $"document.querySelectorAll('ul li').forEach(li => {{ if (li.getAttribute('data-shard') === 'Holodeck') li.textContent = '{holodeckStr}'; if (li.getAttribute('data-shard') === 'Tribble') li.textContent = '{tribbleStr}'; }});\n";
        var request = CreateEvaluateRequest(expression);
        await SendWebSocketRequestAsync(request);
    }
    
    public static async Task ChangeLogoAsync(string newLogo)
    {
        var expression = $"document.querySelectorAll('img').forEach(img => {{ if (img.src.endsWith('/static/img/sto/logo.png')) {{ img.src = '{newLogo}'; img.style.height = '140px'; }} }});";
        var request = CreateEvaluateRequest(expression);
        await SendWebSocketRequestAsync(request);
    }
    
    public static async Task ChangeHintAsync(string newHint)
    {
        try
        {
            var expression = $"document.querySelectorAll('h2').forEach(h2 => {{ if (h2.textContent.trim() === 'Log in with your account') h2.textContent = '{newHint}'; }});\n";
            var request = CreateEvaluateRequest(expression);
            await SendWebSocketRequestAsync(request);
        }
        catch (Exception ex)
        {
            Log.Error($"Error sending evaluate request: {ex.Message}", ex);
        }
    }
    
    public static async Task ChangeAccAndPwdPlaceHolderAsync(string acc, string pwd)
    {
        try
        {
            var expression = $"document.querySelectorAll('input').forEach(input => {{ if (input.type === 'text' && input.name === 'username' && input.placeholder === 'Account Name / Email') input.placeholder = '{acc}'; if (input.type === 'password' && input.name === 'password' && input.placeholder === 'Password') input.placeholder = '{pwd}'; }});\n";
            var request = CreateEvaluateRequest(expression);
            await SendWebSocketRequestAsync(request);
        }
        catch (Exception ex)
        {
            Log.Error($"Error sending evaluate request: {ex.Message}", ex);
        }
    }
    
    public static async Task ChangeH2ContentAsync(string value, string target)
    {
        try
        {
            var expression = $"document.querySelectorAll('h2').forEach(h2 => {{ if (h2.textContent.trim() === '{value}') h2.textContent = '{target}'; }});\n";
            var request = CreateEvaluateRequest(expression);
            await SendWebSocketRequestAsync(request);
        }
        catch (Exception ex)
        {
            Log.Error($"Error sending evaluate request: {ex.Message}", ex);
        }
    }
    
    public static async Task ChangeHrefContentAsync(string href, string value, string target)
    {
        var expression = $"document.querySelectorAll('a').forEach(a => {{ if (a.href === '{href}' && a.textContent.trim() === '{value}') a.textContent = '{target}'; }});";
        var request = CreateEvaluateRequest(expression);
        await SendWebSocketRequestAsync(request);
    }
    
    public static async Task GetLocationAsync()
    {
        var expression = "window.location;";
        var request = CreateEvaluateRequest(expression);
        await SendWebSocketRequestAsync(request);
    }
    
    public static void InitWebSocket(string wsUrl)
    {
        try
        {
            if (WebSocket == null || !WebSocket.IsAlive)
            {
                WebSocket = new WebSocket(wsUrl);
                WebSocket.OnMessage += OnMessage;

                WebSocket.Connect();
                Log.Info($"Initializing WebSocket to {wsUrl}");
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error initializing WebSocket: {ex.Message}", ex);
        }
    }

    private static void OnMessage(object? sender, MessageEventArgs ev)
    {
        try
        {
            JObject response = JObject.Parse(ev.Data);
            Log.Info($"Response received: {response}");
            ProcessResponse(response);
        }
        catch (Exception ex)
        {
            Log.Error($"Error processing message: {ex.Message}", ex);
        }
    }
    
    private static void ProcessResponse(JObject response)
    {
        if (response["result"] != null && response["result"]["result"] != null)
        {
            var result = response["result"]["result"];
            
            if (result["value"] != null)
            {
                string value = result["value"].ToString().ToLowerInvariant();
                
                if (value.Contains("complete"))
                {
                    GlobalVariables.IsLoaded = true;
                }
                else
                {
                    GlobalVariables.IsLoaded = false;
                }

                if (value.Contains("http"))
                {
                    GlobalVariables.ReceivedValue = value;
                }
            }
            
            if (result["objectId"] != null)
            {
                GlobalVariables.ObjectId = result["objectId"].ToString();
            }
            /*if (result["value"] != null && result["value"].ToString().Contains("<html>"))
            {
                string htmlContent = result["value"].ToString();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);
                GlobalVariables.ReceivedValue = htmlDoc.DocumentNode.OuterHtml;
            }*/
        }
    }
}
