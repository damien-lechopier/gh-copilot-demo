import { Router } from 'express';
import { AlbumController } from '@/controllers/AlbumController';
import { handleValidationErrors } from '@/middleware/errorHandler';
import {
  createAlbumValidation,
  updateAlbumValidation,
  getAlbumByIdValidation,
  getAlbumsByYearValidation
} from '@/validators/albumValidators';

const router = Router();
const albumController = new AlbumController();

// GET /albums - Récupérer tous les albums
router.get('/', albumController.getAllAlbums);

// GET /albums/year/:year - Récupérer les albums par année (doit être avant /:id)
router.get('/year/:year', getAlbumsByYearValidation, handleValidationErrors, albumController.getAlbumsByYear);

// GET /albums/:id - Récupérer un album par ID
router.get('/:id', getAlbumByIdValidation, handleValidationErrors, albumController.getAlbumById);

// POST /albums - Créer un nouvel album
router.post('/', createAlbumValidation, handleValidationErrors, albumController.createAlbum);

// PUT /albums/:id - Mettre à jour un album
router.put('/:id', updateAlbumValidation, handleValidationErrors, albumController.updateAlbum);

// DELETE /albums/:id - Supprimer un album
router.delete('/:id', getAlbumByIdValidation, handleValidationErrors, albumController.deleteAlbum);

export default router;