export interface Album {
  id: number;
  title: string;
  artist: string;
  price: number;
  image_url: string;
  year: number;
}

export interface CreateAlbumRequest {
  title: string;
  artist: string;
  price: number;
  image_url: string;
  year?: number;
}

export interface UpdateAlbumRequest {
  title?: string;
  artist?: string;
  price?: number;
  image_url?: string;
  year?: number;
}

export interface ApiResponse<T = any> {
  success: boolean;
  data?: T;
  message?: string;
  error?: string;
}

export interface ValidationError {
  field: string;
  message: string;
}