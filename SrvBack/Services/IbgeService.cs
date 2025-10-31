using System.Net.Http.Json;
using SrvBack.Models;
using System.Linq;

namespace SrvBack.Services
{
    public class IbgeService
    {
        private readonly HttpClient _http;

        public IbgeService(HttpClient http)
        {
            _http = http;
        }

        // ======= Países =======
        public async Task<List<Pais>> GetPaisesAsync()
        {
            var url = "https://servicodados.ibge.gov.br/api/v1/localidades/paises";
            return await _http.GetFromJsonAsync<List<Pais>>(url) ?? new List<Pais>();
        }

        // ======= Regiões =======
        public async Task<List<Regiao>> GetRegioesAsync()
        {
            var url = "https://servicodados.ibge.gov.br/api/v1/localidades/regioes";
            return await _http.GetFromJsonAsync<List<Regiao>>(url) ?? new List<Regiao>();
        }

        // ======= Estados =======
        public async Task<List<Estado>> GetEstadosPorRegiaoAsync(int regiaoId)
        {
            var url = $"https://servicodados.ibge.gov.br/api/v1/localidades/regioes/{regiaoId}/estados";
            return await _http.GetFromJsonAsync<List<Estado>>(url) ?? new List<Estado>();
        }

        // ======= Cidades por UF =======
        public async Task<List<Cidade>> GetCidadesPorUfAsync(int ufId)
        {
            var url = $"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{ufId}/municipios";
            return await _http.GetFromJsonAsync<List<Cidade>>(url) ?? new List<Cidade>();
        }

        public async Task<List<Cidade>> GetDistritosPorUfAsync(int ufId)
        {
            var url = $"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{ufId}/distritos";
            return await _http.GetFromJsonAsync<List<Cidade>>(url) ?? new List<Cidade>();
        }

        public async Task<List<Cidade>> GetSubdistritosPorUfAsync(int ufId)
        {
            var url = $"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{ufId}/subdistritos";
            return await _http.GetFromJsonAsync<List<Cidade>>(url) ?? new List<Cidade>();
        }

        public async Task<List<Cidade>> GetMesorregioesPorUfAsync(int ufId)
        {
            var url = $"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{ufId}/mesorregioes";
            return await _http.GetFromJsonAsync<List<Cidade>>(url) ?? new List<Cidade>();
        }

        public async Task<List<Cidade>> GetMicrorregioesPorUfAsync(int ufId)
        {
            var url = $"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{ufId}/microrregioes";
            return await _http.GetFromJsonAsync<List<Cidade>>(url) ?? new List<Cidade>();
        }

        public async Task<List<Cidade>> GetRegioesImediatasPorUfAsync(int ufId)
        {
            var url = $"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{ufId}/regioes-imediatas";
            return await _http.GetFromJsonAsync<List<Cidade>>(url) ?? new List<Cidade>();
        }

        public async Task<List<Cidade>> GetRegioesIntermediariasPorUfAsync(int ufId)
        {
            var url = $"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{ufId}/regioes-intermediarias";
            return await _http.GetFromJsonAsync<List<Cidade>>(url) ?? new List<Cidade>();
        }

        public async Task<List<Cidade>> GetRegioesMetropolitanasPorUfAsync(int ufId)
        {
            var url = $"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{ufId}/regioes-metropolitanas";
            return await _http.GetFromJsonAsync<List<Cidade>>(url) ?? new List<Cidade>();
        }

        public async Task<List<Cidade>> GetAglomeracoesUrbanasPorUfAsync(int ufId)
        {
            var url = $"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{ufId}/aglomeracoes-urbanas";
            return await _http.GetFromJsonAsync<List<Cidade>>(url) ?? new List<Cidade>();
        }

        // ======= Unificação otimizada =======
        public async Task<List<Cidade>> GetCidadesPorUfUnificadoAsync(int ufId)
        {
            var tasks = new List<Task<IEnumerable<Cidade>>>
            {
                GetCidadesPorUfAsync(ufId).ContinueWith(t => t.Result.Select(c => new Cidade { Id = c.Id, Nome = c.Nome, EstadoId = ufId.ToString() })),
                GetDistritosPorUfAsync(ufId).ContinueWith(t => t.Result.Select(c => new Cidade { Id = c.Id, Nome = c.Nome, EstadoId = ufId.ToString() })),
                GetSubdistritosPorUfAsync(ufId).ContinueWith(t => t.Result.Select(c => new Cidade { Id = c.Id, Nome = c.Nome, EstadoId = ufId.ToString() })),
                GetMesorregioesPorUfAsync(ufId).ContinueWith(t => t.Result.Select(c => new Cidade { Id = c.Id, Nome = c.Nome, EstadoId = ufId.ToString() })),
                GetMicrorregioesPorUfAsync(ufId).ContinueWith(t => t.Result.Select(c => new Cidade { Id = c.Id, Nome = c.Nome, EstadoId = ufId.ToString() })),
                GetRegioesImediatasPorUfAsync(ufId).ContinueWith(t => t.Result.Select(c => new Cidade { Id = c.Id, Nome = c.Nome, EstadoId = ufId.ToString() })),
                GetRegioesIntermediariasPorUfAsync(ufId).ContinueWith(t => t.Result.Select(c => new Cidade { Id = c.Id, Nome = c.Nome, EstadoId = ufId.ToString() })),
                GetRegioesMetropolitanasPorUfAsync(ufId).ContinueWith(t => t.Result.Select(c => new Cidade { Id = c.Id, Nome = c.Nome, EstadoId = ufId.ToString() })),
                GetAglomeracoesUrbanasPorUfAsync(ufId).ContinueWith(t => t.Result.Select(c => new Cidade { Id = c.Id, Nome = c.Nome, EstadoId = ufId.ToString() }))
            };

            var resultados = await Task.WhenAll(tasks);
            return resultados
                .SelectMany(r => r)
                .DistinctBy(c => c.Id)
                .OrderBy(c => c.Nome)
                .ToList();
        }
    }
}
