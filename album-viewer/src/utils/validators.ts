/**
 * Valide une date au format français (JJ/MM/AAAA) et la convertit en objet Date.
 * Retourne l'objet Date si valide, sinon null.
 */
export function parseFrenchDate(input: string): Date | null {
  // Expression régulière pour le format JJ/MM/AAAA
  const regex = /^(\d{2})\/(\d{2})\/(\d{4})$/;
  const match = input.match(regex);
  if (!match) return null;

  const day = parseInt(match[1], 10);
  const month = parseInt(match[2], 10) - 1; // Les mois commencent à 0 en JS
  const year = parseInt(match[3], 10);

  const date = new Date(year, month, day);

  // Vérifie que la date est valide (ex: 31/02/2023 n'existe pas)
  if (
    date.getFullYear() !== year ||
    date.getMonth() !== month ||
    date.getDate() !== day
  ) {
    return null;
  }

  return date;
}

/**
 * Valide si une chaîne est un GUID (UUID v4 ou v1).
 * Retourne true si valide, sinon false.
 */
export function isValidGuid(input: string): boolean {
  const regex = /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[1-5][0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}$/;
  return regex.test(input);
}

/**
 * Valide si une chaîne est une adresse IPv6 valide.
 * Retourne true si valide, sinon false.
 */
export function isValidIPv6(input: string): boolean {
  // Expression régulière pour IPv6 (standard, compressée, IPv4-mapped)
  const regex = /^(?:[A-Fa-f0-9]{1,4}:){7}[A-Fa-f0-9]{1,4}$|^(?:[A-Fa-f0-9]{1,4}:){1,7}:$|^:(?::[A-Fa-f0-9]{1,4}){1,7}$|^(?:[A-Fa-f0-9]{1,4}:){1,6}:[A-Fa-f0-9]{1,4}$|^(?:[A-Fa-f0-9]{1,4}:){1,5}(?::[A-Fa-f0-9]{1,4}){1,2}$|^(?:[A-Fa-f0-9]{1,4}:){1,4}(?::[A-Fa-f0-9]{1,4}){1,3}$|^(?:[A-Fa-f0-9]{1,4}:){1,3}(?::[A-Fa-f0-9]{1,4}){1,4}$|^(?:[A-Fa-f0-9]{1,4}:){1,2}(?::[A-Fa-f0-9]{1,4}){1,5}$|^[A-Fa-f0-9]{1,4}:(?::[A-Fa-f0-9]{1,4}){1,6}$|^:(?::[A-Fa-f0-9]{1,4}){1,7}$|^(?:[A-Fa-f0-9]{1,4}:){1,7}:$|^(?:[A-Fa-f0-9]{1,4}:){6}(?:[0-9]{1,3}\.){3}[0-9]{1,3}$/;
  return regex.test(input);
}