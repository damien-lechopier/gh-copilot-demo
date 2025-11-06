import { body, param } from 'express-validator';

export const createAlbumValidation = [
  body('title')
    .notEmpty()
    .withMessage('Title is required')
    .isLength({ min: 1, max: 200 })
    .withMessage('Title must be between 1 and 200 characters'),
  
  body('artist')
    .notEmpty()
    .withMessage('Artist is required')
    .isLength({ min: 1, max: 100 })
    .withMessage('Artist must be between 1 and 100 characters'),
  
  body('price')
    .isNumeric()
    .withMessage('Price must be a number')
    .isFloat({ min: 0 })
    .withMessage('Price must be positive'),
  
  body('image_url')
    .notEmpty()
    .withMessage('Image URL is required')
    .isURL()
    .withMessage('Image URL must be a valid URL'),
  
  body('year')
    .optional()
    .isInt({ min: 1900, max: new Date().getFullYear() })
    .withMessage(`Year must be between 1900 and ${new Date().getFullYear()}`)
];

export const updateAlbumValidation = [
  param('id')
    .isInt({ min: 1 })
    .withMessage('Album ID must be a positive integer'),
  
  body('title')
    .optional()
    .isLength({ min: 1, max: 200 })
    .withMessage('Title must be between 1 and 200 characters'),
  
  body('artist')
    .optional()
    .isLength({ min: 1, max: 100 })
    .withMessage('Artist must be between 1 and 100 characters'),
  
  body('price')
    .optional()
    .isNumeric()
    .withMessage('Price must be a number')
    .isFloat({ min: 0 })
    .withMessage('Price must be positive'),
  
  body('image_url')
    .optional()
    .isURL()
    .withMessage('Image URL must be a valid URL'),
  
  body('year')
    .optional()
    .isInt({ min: 1900, max: new Date().getFullYear() })
    .withMessage(`Year must be between 1900 and ${new Date().getFullYear()}`)
];

export const getAlbumByIdValidation = [
  param('id')
    .isInt({ min: 1 })
    .withMessage('Album ID must be a positive integer')
];

export const getAlbumsByYearValidation = [
  param('year')
    .isInt({ min: 1900, max: new Date().getFullYear() })
    .withMessage(`Year must be between 1900 and ${new Date().getFullYear()}`)
];