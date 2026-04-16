export interface GameSummary {
  id: number;
  name: string;
  genre: string;
  price: number;
  releaseDate: string; // ISO string from API (DateOnly serializes as 'YYYY-MM-DD')
}

export interface GameDetails {
  id: number;
  name: string;
  genreId: number;
  price: number;
  releaseDate: string;
}

export interface Genre {
  id: number;
  name: string;
}

export interface CreateGameDto {
  name: string;
  genreId: number;
  price: number;
  releaseDate: string;
}
