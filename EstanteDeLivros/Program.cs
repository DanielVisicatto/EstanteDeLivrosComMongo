using EstanteDeLivros.Database;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

int op;

MongoClient mongo = new MongoClient("mongodb://localhost:27017");
var database = mongo.GetDatabase("EstanteDeLivros");
var collection = database.GetCollection<BsonDocument>("Livros");

do
{
    Console.Clear();
    op = Menu();
    switch (op)
    {
        default:
            Console.WriteLine("Opção inválida!");
           
            break;

        case 1:
            PreencheFormulario();            
            
            break;

        case 2:
            Console.WriteLine("Digite o titulo do livro");
            BuscarPorTitulo();
            
            break;

        case 3:
            EditarLivro();
            break;

        case 4:

            DeletarLivro();
            break;

          case 5:
            BuscarTodos();
            break;

        case 6:
            Console.WriteLine("Obrigado por utilizar os nossos serviços!");
            Environment.Exit(0);
            break;
    }
} while (op != 6);

void EditarLivro()
{
    Console.Clear();
    Console.WriteLine("Informe o título do livro para editar");
    var titulo = Console.ReadLine();

    var filtro = Builders<BsonDocument>.Filter.Regex("Titulo", titulo);
    var livros = collection.Find(filtro).ToList();    

    int opcao = 0;
    do
    {
        livros.ForEach(x => Console.WriteLine(BsonSerializer.Deserialize<LivroDB>(x)));        
        opcao = SubMenu();
        switch (opcao)
        {
            
            default: Console.WriteLine("Opção inválida!");
                break;
            case 1:
                Console.WriteLine("Informe o novo Título");
                var novoTitulo = Console.ReadLine();              
                var atualizacao = Builders<BsonDocument>.Update.Set("Titulo", novoTitulo);

                collection.UpdateOne(filtro, atualizacao);
                filtro = Builders<BsonDocument>.Filter.Regex("Titulo", novoTitulo);
                Thread.Sleep(1200);
                Console.WriteLine("Informação do título atualizada!");
                break;

            case 2:
                Console.WriteLine("Informe novamente os Autores");
                var novosautores = Console.ReadLine();
                
                var atualizacaoAutores = Builders<BsonDocument>.Update.Set("Autores", novosautores);

                collection.UpdateOne(filtro, atualizacaoAutores);
                Thread.Sleep(1200);
                Console.WriteLine("Informação de autores atualizada!");
                break;

            case 3:
                Console.WriteLine("Informe nova Editora");
                var novaEditora = Console.ReadLine();
             
                var atualizacaoEditora = Builders<BsonDocument>.Update.Set("Editora", novaEditora);

                collection.UpdateOne(filtro, atualizacaoEditora);
                Thread.Sleep(1200);
                Console.WriteLine("Informação da editora atualizada!");
                break;

            case 4:
                Console.WriteLine("Informe novamente o Código ISBN");
                Thread.Sleep(1200);
                var novoISBN = Console.ReadLine();
                               
                var atualizacaoISBN = Builders<BsonDocument>.Update.Set("Codigo_ISBN", novoISBN);

                collection.UpdateOne(filtro, atualizacaoISBN);
                Thread.Sleep(1200);
                Console.WriteLine("ISBN atualizado!");
                break;

            case 5:
                Console.WriteLine("Ja leu este livro? (S / N)");
                var resposta = Console.ReadLine();
                var novoEstado = resposta.ToLower() == "s" ? true : false;
                var atualizacaoEstado = Builders<BsonDocument>.Update.Set("JaLido", novoEstado);
                collection.UpdateOne(filtro, atualizacaoEstado);
                Thread.Sleep(1200);
                Console.WriteLine("Estado atualizado!"); 
                break;

            case 6:                
                break;
        }       
    } 
    while (opcao != 6);
    
}
void DeletarLivro()
{
    Console.WriteLine("Informe o título do livro");
    var titulo = Console.ReadLine();
    var filter = Builders<BsonDocument>.Filter.Eq("Titulo", titulo);
    Console.WriteLine($"Deseja realmente deletar o livro: {collection.Find(filter)}?\n" +
                      "Esta Operação não podera ser revertida!!\n" +
                      "Digite CONFIRMAR (em letras maiusculas) para DELETAR ou qualquer telca para voltar. ");
    var resposta = Console.ReadLine();
    if (resposta == "CONFIRMAR")
        collection.FindOneAndDelete(filter);
    else return;
}
LivroDB CadastrarLivro(LivroDB liv)
{       
    var registroLivro = new BsonDocument
{
    {"Titulo", liv.Titulo},
    {"Autores", liv.Autores},
    {"Editora", liv.Editora},
    {"Codigo_ISBN", liv.ISBN},
    {"JaLido", liv.Lido},
};
    collection.InsertOne(registroLivro);
    Console.WriteLine("Livro Cadastrado!!");
    Thread.Sleep(2500);
    return liv;
}
void BuscarPorTitulo()
{    
    var titulo = Console.ReadLine();
    var filtro = Builders<BsonDocument>.Filter.Regex("Titulo", titulo);
    var livros = collection.Find(filtro).ToList();
    livros.ForEach(x => Console.WriteLine(BsonSerializer.Deserialize<LivroDB>(x)));
    Console.ReadLine();   
}
void BuscarTodos()
{    
    var livros = collection.Find( _=> true).ToList();
    Console.Clear();
    livros.ForEach(x => Console.WriteLine(BsonSerializer.Deserialize<LivroDB>(x)));    
    Console.ReadLine();
}
int Menu()
{
    

    Console.WriteLine("______________________________________________");
    Console.WriteLine("|                                             |");
    Console.WriteLine("|               Estante de Livros             |");
    Console.WriteLine("|_____________________________________________|");
    Console.WriteLine("|*********************************************|");
    Console.WriteLine("|*                                           *|");
    Console.WriteLine("|*          Selecione a opção desejada       *|");
    Console.WriteLine("|*___________________________________________*|");
    Console.WriteLine("|*   1  -  Cadastrar um novo livro           *|");
    Console.WriteLine("|*                                           *|");
    Console.WriteLine("|*   2  -  Buscar por um livro               *|");
    Console.WriteLine("|*                                           *|");
    Console.WriteLine("|*   3  -  Editar dados de um livro          *|");
    Console.WriteLine("|*                                           *|");
    Console.WriteLine("|*   4  -  Deletar um livro                  *|");
    Console.WriteLine("|*                                           *|");
    Console.WriteLine("|*   5  -  Trazer todos os livros            *|");
    Console.WriteLine("|*                                           *|");
    Console.WriteLine("|*   6  -  Sair                              *|");
    Console.WriteLine("|*___________________________________________*|");

    if (!int.TryParse(Console.ReadLine(), out var opcao))
    {
        Console.Clear();
        Console.WriteLine("Opção inválida");
        Thread.Sleep(2000);
        Console.Clear();

        return 0;
    }
    else
    {
        return opcao;
    }
        
}
int SubMenu()
{
    Console.WriteLine("___________________________________");
    Console.WriteLine("|*********************************|");
    Console.WriteLine("|*                               *|");
    Console.WriteLine("|*    O que deseja editar?       *|");
    Console.WriteLine("|*_______________________________*|");
    Console.WriteLine("|*   1  -  Titulo                *|");
    Console.WriteLine("|*                               *|");
    Console.WriteLine("|*   2  -  Autor(es)             *|");
    Console.WriteLine("|*                               *|");
    Console.WriteLine("|*   3  -  Editora               *|");
    Console.WriteLine("|*                               *|");
    Console.WriteLine("|*   4  -  ISBN                  *|");
    Console.WriteLine("|*                               *|");
    Console.WriteLine("|*   5  -  Estado                *|");
    Console.WriteLine("|*                               *|");
    Console.WriteLine("|*   6  -  Voltar                *|");
    Console.WriteLine("|*_______________________________*|");

    if (!int.TryParse(Console.ReadLine(), out var subOpcao))
    {
        Console.Clear();
        Console.WriteLine("Opção inválida");
        Thread.Sleep(2000);
        Console.Clear();

        return 0;
    }
    else
    {
        return subOpcao;
    }
        
}
void PreencheFormulario()
{
    Console.WriteLine("Entre com os dados do livro:");
    Console.Write("Título: ");
    var titulo = Console.ReadLine();
    Console.Write("Autor(es): ");
    var autores = Console.ReadLine();
    Console.Write("Editora: ");
    var editora = Console.ReadLine();
    Console.Write("ISBN: ");
    var iSBN = Console.ReadLine();
    Console.Write("Já Lido? (S / para sim, ou qqr tecla para não)");
    string jaLi = Console.ReadLine();
    var lido = LivroDB.VerificarLido(jaLi);
    LivroDB  livro = new(titulo, autores, editora, iSBN, lido);
    CadastrarLivro(livro);
}