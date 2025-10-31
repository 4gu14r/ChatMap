using Microsoft.AspNetCore.SignalR;
using SrvBack.Services;
using SharedModels;

namespace SrvBack.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IbgeService _ibgeService;

        // Histórico de mensagens em memória
        private static readonly Dictionary<string, List<Mensagem>> mensagens =
            new Dictionary<string, List<Mensagem>>();

        public ChatHub(IbgeService ibgeService)
        {
            _ibgeService = ibgeService;
        }

        // Retorna a lista de canais
        public async Task<List<string>> GetCanaisDisponiveis()
        {
            var paises = await _ibgeService.GetPaisesAsync();
            return paises.Select(p => p.Nome).ToList();
        }

        // Entra no canal (mensagem do sistema entra no histórico)
        public async Task EntrarNoCanal(string canal)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, canal);

            if (!mensagens.ContainsKey(canal))
                mensagens[canal] = new List<Mensagem>();

            mensagens[canal].Add(new Mensagem
            {
                Usuario = "Sistema",
                Texto = $"{Context.ConnectionId} entrou no canal {canal}"
            });
        }

        // Sai do canal (mensagem do sistema entra no histórico e é enviada)
        public async Task SairDoCanal(string canal)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, canal);

            if (!mensagens.ContainsKey(canal))
                mensagens[canal] = new List<Mensagem>();

            var msg = new Mensagem
            {
                Usuario = "Sistema",
                Texto = $"{Context.ConnectionId} saiu do canal {canal}"
            };

            mensagens[canal].Add(msg);

            await Clients.Group(canal).SendAsync("ReceiveMessage", msg);
        }

        // Envia mensagem
        public async Task EnviarMensagem(string canal, string usuario, string texto)
        {
            if (!mensagens.ContainsKey(canal))
                mensagens[canal] = new List<Mensagem>();

            var msg = new Mensagem { Usuario = usuario, Texto = texto };
            mensagens[canal].Add(msg);

            await Clients.Group(canal).SendAsync("ReceiveMessage", msg);
        }

        // Retorna o histórico do canal
        public Task<List<Mensagem>> GetMensagensDoCanal(string canal)
        {
            if (mensagens.TryGetValue(canal, out var lista))
                return Task.FromResult(lista.ToList());

            return Task.FromResult(new List<Mensagem>());
        }
    }
}
