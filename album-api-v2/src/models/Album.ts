import { Album, CreateAlbumRequest, UpdateAlbumRequest } from '@/types/album';

class AlbumModel {
  private static albums: Album[] = [
    {
      id: 1,
      title: "You, Me and an App Id",
      artist: "Daprize",
      price: 10.99,
      image_url: "https://aka.ms/albums-daprlogo",
      year: 2023
    },
    {
      id: 2,
      title: "Seven Revision Army",
      artist: "The Blue-Green Stripes",
      price: 13.99,
      image_url: "https://aka.ms/albums-containerappslogo",
      year: 2022
    },
    {
      id: 3,
      title: "Scale It Up",
      artist: "KEDA Club",
      price: 13.99,
      image_url: "https://aka.ms/albums-kedalogo",
      year: 2023
    },
    {
      id: 4,
      title: "Lost in Translation",
      artist: "MegaDNS",
      price: 12.99,
      image_url: "https://aka.ms/albums-envoylogo",
      year: 2021
    },
    {
      id: 5,
      title: "Lock Down Your Love",
      artist: "V is for VNET",
      price: 12.99,
      image_url: "https://aka.ms/albums-vnetlogo",
      year: 2022
    },
    {
      id: 6,
      title: "Sweet Container O' Mine",
      artist: "Guns N Probeses",
      price: 14.99,
      image_url: "https://aka.ms/albums-containerappslogo",
      year: 2023
    }
  ];

  static getAll(): Album[] {
    return [...this.albums];
  }

  static getById(id: number): Album | null {
    return this.albums.find(album => album.id === id) || null;
  }

  static getByYear(year: number): Album[] {
    return this.albums.filter(album => album.year === year);
  }

  static create(albumData: CreateAlbumRequest): Album {
    const newId = this.albums.length > 0 ? Math.max(...this.albums.map(a => a.id)) + 1 : 1;
    const newAlbum: Album = {
      id: newId,
      title: albumData.title,
      artist: albumData.artist,
      price: albumData.price,
      image_url: albumData.image_url,
      year: albumData.year || new Date().getFullYear()
    };
    
    this.albums.push(newAlbum);
    return newAlbum;
  }

  static update(id: number, albumData: UpdateAlbumRequest): Album | null {
    const index = this.albums.findIndex(album => album.id === id);
    if (index === -1) {
      return null;
    }

    const existingAlbum = this.albums[index];
    const updatedAlbum: Album = {
      ...existingAlbum,
      ...albumData,
      id // Ensure ID cannot be changed
    };

    this.albums[index] = updatedAlbum;
    return updatedAlbum;
  }

  static delete(id: number): boolean {
    const index = this.albums.findIndex(album => album.id === id);
    if (index === -1) {
      return false;
    }

    this.albums.splice(index, 1);
    return true;
  }

  static count(): number {
    return this.albums.length;
  }

  static reset(): void {
    this.albums = [
      {
        id: 1,
        title: "You, Me and an App Id",
        artist: "Daprize",
        price: 10.99,
        image_url: "https://aka.ms/albums-daprlogo",
        year: 2023
      },
      {
        id: 2,
        title: "Seven Revision Army",
        artist: "The Blue-Green Stripes",
        price: 13.99,
        image_url: "https://aka.ms/albums-containerappslogo",
        year: 2022
      },
      {
        id: 3,
        title: "Scale It Up",
        artist: "KEDA Club",
        price: 13.99,
        image_url: "https://aka.ms/albums-kedalogo",
        year: 2023
      },
      {
        id: 4,
        title: "Lost in Translation",
        artist: "MegaDNS",
        price: 12.99,
        image_url: "https://aka.ms/albums-envoylogo",
        year: 2021
      },
      {
        id: 5,
        title: "Lock Down Your Love",
        artist: "V is for VNET",
        price: 12.99,
        image_url: "https://aka.ms/albums-vnetlogo",
        year: 2022
      },
      {
        id: 6,
        title: "Sweet Container O' Mine",
        artist: "Guns N Probeses",
        price: 14.99,
        image_url: "https://aka.ms/albums-containerappslogo",
        year: 2023
      }
    ];
  }
}

export default AlbumModel;