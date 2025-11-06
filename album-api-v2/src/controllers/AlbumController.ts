import { Request, Response } from 'express';
import { AlbumModel } from '@/models/AlbumModel';
import { CreateAlbumRequest, UpdateAlbumRequest, ApiResponse, Album } from '@/types/album';

export class AlbumController {
  private albumModel: AlbumModel;

  constructor() {
    this.albumModel = new AlbumModel();
  }

  // GET /albums - Récupérer tous les albums
  public getAllAlbums = (req: Request, res: Response): void => {
    try {
      const albums = this.albumModel.getAllAlbums();
      
      const response: ApiResponse<Album[]> = {
        success: true,
        data: albums,
        message: `Retrieved ${albums.length} albums`
      };
      
      res.status(200).json(response);
    } catch (error) {
      const response: ApiResponse = {
        success: false,
        error: 'Failed to retrieve albums'
      };
      res.status(500).json(response);
    }
  };

  // GET /albums/:id - Récupérer un album par ID
  public getAlbumById = (req: Request, res: Response): void => {
    try {
      const id = parseInt(req.params.id);
      const album = this.albumModel.getAlbumById(id);
      
      if (!album) {
        const response: ApiResponse = {
          success: false,
          message: `Album with ID ${id} not found`
        };
        res.status(404).json(response);
        return;
      }
      
      const response: ApiResponse<Album> = {
        success: true,
        data: album
      };
      
      res.status(200).json(response);
    } catch (error) {
      const response: ApiResponse = {
        success: false,
        error: 'Failed to retrieve album'
      };
      res.status(500).json(response);
    }
  };

  // GET /albums/year/:year - Récupérer les albums par année
  public getAlbumsByYear = (req: Request, res: Response): void => {
    try {
      const year = parseInt(req.params.year);
      const albums = this.albumModel.getAlbumsByYear(year);
      
      const response: ApiResponse<Album[]> = {
        success: true,
        data: albums,
        message: `Found ${albums.length} albums from ${year}`
      };
      
      res.status(200).json(response);
    } catch (error) {
      const response: ApiResponse = {
        success: false,
        error: 'Failed to retrieve albums by year'
      };
      res.status(500).json(response);
    }
  };

  // POST /albums - Créer un nouvel album
  public createAlbum = (req: Request, res: Response): void => {
    try {
      const albumData: CreateAlbumRequest = req.body;
      const newAlbum = this.albumModel.createAlbum(albumData);
      
      const response: ApiResponse<Album> = {
        success: true,
        data: newAlbum,
        message: 'Album created successfully'
      };
      
      res.status(201).json(response);
    } catch (error) {
      const response: ApiResponse = {
        success: false,
        error: error instanceof Error ? error.message : 'Failed to create album'
      };
      res.status(400).json(response);
    }
  };

  // PUT /albums/:id - Mettre à jour un album
  public updateAlbum = (req: Request, res: Response): void => {
    try {
      const id = parseInt(req.params.id);
      const albumData: UpdateAlbumRequest = req.body;
      const updatedAlbum = this.albumModel.updateAlbum(id, albumData);
      
      if (!updatedAlbum) {
        const response: ApiResponse = {
          success: false,
          message: `Album with ID ${id} not found`
        };
        res.status(404).json(response);
        return;
      }
      
      const response: ApiResponse<Album> = {
        success: true,
        data: updatedAlbum,
        message: 'Album updated successfully'
      };
      
      res.status(200).json(response);
    } catch (error) {
      const response: ApiResponse = {
        success: false,
        error: error instanceof Error ? error.message : 'Failed to update album'
      };
      res.status(400).json(response);
    }
  };

  // DELETE /albums/:id - Supprimer un album
  public deleteAlbum = (req: Request, res: Response): void => {
    try {
      const id = parseInt(req.params.id);
      const deleted = this.albumModel.deleteAlbum(id);
      
      if (!deleted) {
        const response: ApiResponse = {
          success: false,
          message: `Album with ID ${id} not found`
        };
        res.status(404).json(response);
        return;
      }
      
      const response: ApiResponse = {
        success: true,
        message: 'Album deleted successfully'
      };
      
      res.status(200).json(response);
    } catch (error) {
      const response: ApiResponse = {
        success: false,
        error: 'Failed to delete album'
      };
      res.status(500).json(response);
    }
  };
}