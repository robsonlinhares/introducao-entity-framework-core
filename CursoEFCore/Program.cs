using System;
using System.Collections.Generic;
using System.Linq;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {

            // using var db = new Data.ApplicationContext();

            // //db.Database.Migrate();

            // var existe = db.Database.GetPendingMigrations().Any();
            // if (existe)
            // {
            //     //regra de negocio
            // }

            //InserirDados();
            //InserirDadosEmMassa();
            //ConsultarDados();
            //CadastrarPedido();
            //ConsultarPedidoCarregamentoAdiantado();
            //AtualizarDados();
            RemoverDados();
        }

        private static void RemoverDados()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.Find(2);

            db.Clientes.Remove(cliente);
            //db.Remove(cliente);
            //db.Entry(cliente).State = EntityState.Deleted;

            db.SaveChanges();

        }

        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.Find(1);

            cliente.Nome = "Robson Linhares";

            //db.Entry(cliente).State = EntityState.Modified;

            //db.Clientes.Update(cliente);
            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            var pedidos = db.Pedidos
            //.Include("Itens")
            .Include(p => p.Itens)
            .ThenInclude(p => p.Produto)
            .ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10
                    }
                }
            };

            db.Pedidos.Add(pedido);
            db.SaveChanges();
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();
            // var consultaPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
            //var consultaPorMetodo = db.Clientes.AsNoTracking().Where(p => p.Id > 0).ToList();
            var consultaPorMetodo = db.Clientes
            .Where(p => p.Id > 0)
            .OrderBy(p => p.Id)
            .ToList();

            foreach (var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando cliente: {cliente.Id}");
                //db.Clientes.Find(cliente.Id);
                db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567",
                Valor = "10",
                TipoProduto = TipoProduto.MercsdoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Robson Linhares",
                CEP = "11730000",
                Cidade = "Mongaguá",
                Estado = "SP",
                Telefone = "9988888990"
            };


            var listaCliente = new[]
            {
                new Cliente
            {
                Nome = "Teste 1",
                CEP = "11730000",
                Cidade = "Mongaguá",
                Estado = "SP",
                Telefone = "9988888990"
            },
              new Cliente
            {
                Nome = "Teste 2",
                CEP = "11730000",
                Cidade = "Mongaguá",
                Estado = "SP",
                Telefone = "9988888990"
            }
        };

            using var db = new Data.ApplicationContext();
            //db.AddRange(produto, cliente);
            //db.Clientes.AddRange(listaCliente);
            db.Set<Cliente>().AddRange(listaCliente);


            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registros: {registros}");
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567",
                Valor = "10",
                TipoProduto = TipoProduto.MercsdoriaParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();
            //db.Produtos.Add(produto);
            //db.Set<Produto>().Add(produto);
            //db.Entry(produto).State = EntityState.Added;
            db.Add(produto);
            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registros: {registros}");
        }
    }
}
