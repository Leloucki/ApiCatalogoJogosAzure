using ApiCatalogoJogos.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private readonly SqlConnection sqlConnection;

        public JogoRepository(IConfiguration configuration)
        {
            sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
        }

        public async Task Atualizar(Jogo jogo)
        {
            var cmd = $"update Jogos set nome = '{jogo.Nome}', produtora = '{jogo.Produtora}', preco = '{jogo.Preco.ToString().Replace(",", ".")}' where id = '{jogo.Id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }

        public async Task Inserir(Jogo jogo)
        {
            var cmd = $"insert into Jogos (id, nome, produtora, preco) values ('{jogo.Id}', '{jogo.Nome}', '{jogo.Produtora}', '{jogo.Preco}')";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            var jogos = new List<Jogo>();

            var cmd = $"select * from Jogos order by id offset {((pagina - 1) * quantidade)} rows fetch next {quantidade} rows only";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while(sqlDataReader.Read()){
                jogos.Add(new Jogo
                {
                    Id = (Guid)sqlDataReader["id"],
                    Nome = (string)sqlDataReader["nome"],
                    Produtora = (string)sqlDataReader["produtora"],
                    Preco = (double)sqlDataReader["preco"]
                }
                );
            }

            await sqlConnection.CloseAsync();

            return jogos;
        }

        public async Task<Jogo> Obter(Guid Id)
        {
            Jogo jogo = null;

            var cmd = $"select * from Jogos where id = '{Id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                jogo = new Jogo
                {
                    Id = (Guid)sqlDataReader["id"],
                    Nome = (string)sqlDataReader["nome"],
                    Produtora = (string)sqlDataReader["produtora"],
                    Preco = (double)sqlDataReader["preco"]
                };
            }

            await sqlConnection.CloseAsync();

            return jogo;
        }

        public async Task<List<Jogo>> Obter(string nome, string produtora)
        {
            var jogos = new List<Jogo>();

            var cmd = $"select * from Jogos where nome = '{nome}' and produtora = '{produtora}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                jogos.Add(new Jogo
                {
                    Id = (Guid)sqlDataReader["id"],
                    Nome = (string)sqlDataReader["nome"],
                    Produtora = (string)sqlDataReader["produtora"],
                    Preco = (double)sqlDataReader["preco"]
                }
                );
            }

            await sqlConnection.CloseAsync();

            return jogos;
        }

        public async Task Remover(Guid Id)
        {
            var cmd = $"delete from Jogos where id = '{Id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);
            await sqlCommand.ExecuteNonQueryAsync();
            await sqlConnection.CloseAsync();
        }
    }
}
