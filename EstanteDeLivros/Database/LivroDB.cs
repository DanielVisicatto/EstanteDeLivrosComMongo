using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace EstanteDeLivros.Database
{
    internal class LivroDB
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Titulo")]
        public string? Titulo { get; set; }


        [BsonElement("Autores")]
        public string? Autores { get; set; }


        [BsonElement("Editora")]
        public string? Editora { get; set; }

        [BsonElement("Codigo_ISBN")]
        public string? ISBN { get; set; }

        [BsonElement("JaLido")]
        [BsonRepresentation(BsonType.Boolean)]
        public bool Lido { get; set; }

        public LivroDB()
        {
        }
                

        public LivroDB(string? titulo, string? autores, string? editora, string? iSBN, bool lido)
        {           
            Titulo = titulo;
            Autores = autores;
            Editora = editora;
            ISBN = iSBN;
            Lido = lido;
        }

        public static bool VerificarLido(string li)
        {
            if (li.ToLower() == "s") return true;
            return false;
        }

        public override string ToString()
        {
            return $"--------------------------------------------------\n" +
                   $"Informações do Livro:\n" +
                   $"--------------------------------------------------\n" +
                   $"Titulo: {Titulo}\n" +
                   $"Autor(es): {Autores}\n" +
                   $"Edição: {Editora}\n" +
                   $"ISBN: {ISBN}\n" +
                   $"Estado: {Lido}\n" +
                   $"--------------------------------------------------\n";
        }
    }


}
