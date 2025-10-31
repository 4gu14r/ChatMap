using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using SharedModels;

namespace AppFront.Services
{
    public class ChatHubService
    {
        public HubConnection HubConnection { get; private set; }

        public ChatHubService(NavigationManager navigationManager)
        {
            HubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7139/chathub")
                .WithAutomaticReconnect()
                .Build();
        }

        // Inicia a conexão se ainda não estiver conectada
        public async Task StartAsync()
        {
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        // Entra em um canal (grupo)
        public async Task EntrarNoCanal(string canal)
        {
            await HubConnection.InvokeAsync("EntrarNoCanal", canal);
        }

        // Sai de um canal
        public async Task SairDoCanal(string canal)
        {
            await HubConnection.InvokeAsync("SairDoCanal", canal);
        }

        // Envia mensagem
        public async Task EnviarMensagem(string canal, string usuario, string mensagem)
        {
            await HubConnection.InvokeAsync("EnviarMensagem", canal, usuario, mensagem);
        }

        // Retorna todas as mensagens de um canal
        public async Task<List<Mensagem>> GetMensagensDoCanal(string canal)
        {
            return await HubConnection.InvokeAsync<List<Mensagem>>("GetMensagensDoCanal", canal);
        }



        // Retorna a lista de canais disponíveis
        public async Task<List<string>> GetCanaisDisponiveis()
        {
            return await HubConnection.InvokeAsync<List<string>>("GetCanaisDisponiveis");
        }
    }
}
