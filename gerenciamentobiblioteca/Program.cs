using System;
using System.Collections.Generic;

namespace BibliotecaConsole
{
    class Program
    {
        static List<Livro> catalogo = new List<Livro>();
        static Dictionary<string, Usuario> usuarios = new Dictionary<string, Usuario>();

        static void Main(string[] args)
        {
            string opcao;
            do
            {
                Console.Clear();
                Console.WriteLine("=== Sistema de Gerenciamento de Biblioteca ===");
                Console.WriteLine("1. Cadastrar Livro (Administrador)");
                Console.WriteLine("2. Consultar Catálogo");
                Console.WriteLine("3. Emprestar Livro");
                Console.WriteLine("4. Devolver Livro");
                Console.WriteLine("5. Sair");
                Console.Write("Escolha uma opção: ");
                opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        CadastrarLivro();
                        break;
                    case "2":
                        ConsultarCatalogo();
                        break;
                    case "3":
                        EmprestarLivro();
                        break;
                    case "4":
                        DevolverLivro();
                        break;
                }
            } while (opcao != "5");
        }

        static void CadastrarLivro()
        {
            Console.Write("Título: ");
            string titulo = Console.ReadLine();
            Console.Write("Autor: ");
            string autor = Console.ReadLine();
            Console.Write("Gênero: ");
            string genero = Console.ReadLine();
            Console.Write("Quantidade: ");
            int quantidade = int.Parse(Console.ReadLine());

            catalogo.Add(new Livro(titulo, autor, genero, quantidade));
            Console.WriteLine("Livro cadastrado com sucesso!");
            Console.ReadLine();
        }

        static void ConsultarCatalogo()
        {
            Console.WriteLine("=== Catálogo da Biblioteca ===");
            foreach (var livro in catalogo)
            {
                Console.WriteLine($"Título: {livro.Titulo}, Autor: {livro.Autor}, Gênero: {livro.Genero}, Quantidade Disponível: {livro.QuantidadeDisponivel}");
            }
            Console.ReadLine();
        }

        static void EmprestarLivro()
        {
            Console.Write("Digite seu nome de usuário: ");
            string nomeUsuario = Console.ReadLine();

            if (!usuarios.ContainsKey(nomeUsuario))
            {
                usuarios[nomeUsuario] = new Usuario(nomeUsuario);
            }

            Usuario usuario = usuarios[nomeUsuario];

            if (usuario.LivrosEmprestados.Count >= 3)
            {
                Console.WriteLine("Limite de 3 livros emprestados atingido.");
                Console.ReadLine();
                return;
            }

            Console.Write("Título do livro que deseja emprestar: ");
            string titulo = Console.ReadLine();
            Livro livro = catalogo.Find(l => l.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase));

            if (livro != null && livro.QuantidadeDisponivel > 0)
            {
                usuario.EmprestarLivro(livro);
                Console.WriteLine($"Livro '{livro.Titulo}' emprestado com sucesso!");
            }
            else
            {
                Console.WriteLine("Livro não encontrado ou sem estoque disponível.");
            }
            Console.ReadLine();
        }

        static void DevolverLivro()
        {
            Console.Write("Digite seu nome de usuário: ");
            string nomeUsuario = Console.ReadLine();

            if (!usuarios.ContainsKey(nomeUsuario))
            {
                Console.WriteLine("Usuário não encontrado.");
                Console.ReadLine();
                return;
            }

            Usuario usuario = usuarios[nomeUsuario];

            if (usuario.LivrosEmprestados.Count == 0)
            {
                Console.WriteLine("Você não tem livros emprestados.");
                Console.ReadLine();
                return;
            }

            Console.Write("Título do livro que deseja devolver: ");
            string titulo = Console.ReadLine();
            Livro livro = usuario.LivrosEmprestados.Find(l => l.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase));

            if (livro != null)
            {
                usuario.DevolverLivro(livro);
                Console.WriteLine($"Livro '{livro.Titulo}' devolvido com sucesso!");
            }
            else
            {
                Console.WriteLine("Você não possui esse livro.");
            }
            Console.ReadLine();
        }
    }

    class Livro
    {
        public string Titulo { get; }
        public string Autor { get; }
        public string Genero { get; }
        public int QuantidadeDisponivel { get; set; }

        public Livro(string titulo, string autor, string genero, int quantidade)
        {
            Titulo = titulo;
            Autor = autor;
            Genero = genero;
            QuantidadeDisponivel = quantidade;
        }
    }

    class Usuario
    {
        public string Nome { get; }
        public List<Livro> LivrosEmprestados { get; } = new List<Livro>();

        public Usuario(string nome)
        {
            Nome = nome;
        }

        public void EmprestarLivro(Livro livro)
        {
            LivrosEmprestados.Add(livro);
            livro.QuantidadeDisponivel--;
        }

        public void DevolverLivro(Livro livro)
        {
            LivrosEmprestados.Remove(livro);
            livro.QuantidadeDisponivel++;
        }
    }
}