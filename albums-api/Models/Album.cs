namespace albums_api.Models
{
    public record Album(int Id, string Title, string Artist, double Price, string Image_url, int Year = 2023)
    {
        // Static list to simulate a database
        private static List<Album> _albums = new List<Album>(){
            new Album(1, "You, Me and an App Id", "Daprize", 10.99, "https://aka.ms/albums-daprlogo", 2023),
            new Album(2, "Seven Revision Army", "The Blue-Green Stripes", 13.99, "https://aka.ms/albums-containerappslogo", 2022),
            new Album(3, "Scale It Up", "KEDA Club", 13.99, "https://aka.ms/albums-kedalogo", 2023),
            new Album(4, "Lost in Translation", "MegaDNS", 12.99,"https://aka.ms/albums-envoylogo", 2021),
            new Album(5, "Lock Down Your Love", "V is for VNET", 12.99, "https://aka.ms/albums-vnetlogo", 2022),
            new Album(6, "Sweet Container O' Mine", "Guns N Probeses", 14.99, "https://aka.ms/albums-containerappslogo", 2023)
        };

        public static List<Album> GetAll()
        {
            return _albums.ToList();
        }

        public static Album? GetById(int id)
        {
            return _albums.FirstOrDefault(a => a.Id == id);
        }

        public static List<Album> GetByYear(int year)
        {
            return _albums.Where(a => a.Year == year).ToList();
        }

        public static Album Create(Album album)
        {
            var newId = _albums.Any() ? _albums.Max(a => a.Id) + 1 : 1;
            var newAlbum = album with { Id = newId };
            _albums.Add(newAlbum);
            return newAlbum;
        }

        public static Album? Update(int id, Album updatedAlbum)
        {
            var index = _albums.FindIndex(a => a.Id == id);
            if (index == -1) return null;

            var albumToUpdate = updatedAlbum with { Id = id };
            _albums[index] = albumToUpdate;
            return albumToUpdate;
        }

        public static bool Delete(int id)
        {
            var albumIndex = _albums.FindIndex(a => a.Id == id);
            if (albumIndex == -1) return false;

            _albums.RemoveAt(albumIndex);
            return true;
        }
    }
}
