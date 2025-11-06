import express from 'express';
import albumRoutes from '@/routes/albumRoutes';
import { corsMiddleware } from '@/middleware/cors';
import { errorHandler, notFoundHandler } from '@/middleware/errorHandler';

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware global
app.use(corsMiddleware);
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Routes
app.use('/albums', albumRoutes);

// Route de santé
app.get('/health', (req, res) => {
  res.status(200).json({
    success: true,
    message: 'Album API v2 is running',
    timestamp: new Date().toISOString()
  });
});

// Middleware de gestion d'erreurs (doit être en dernier)
app.use(notFoundHandler);
app.use(errorHandler);

// Démarrage du serveur
app.listen(PORT, () => {
  console.log(`🎵 Album API v2 running on port ${PORT}`);
  console.log(`🏥 Health check: http://localhost:${PORT}/health`);
  console.log(`📀 Albums endpoint: http://localhost:${PORT}/albums`);
});

export default app;