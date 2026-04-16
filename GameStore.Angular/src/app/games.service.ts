import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GameSummary, GameDetails, Genre, CreateGameDto } from './game.model';

@Injectable({
  providedIn: 'root'
})
export class GamesService {
  private readonly baseUrl = '/api';

  constructor(private http: HttpClient) {}

  getGames(): Observable<GameSummary[]> {
    return this.http.get<GameSummary[]>(`${this.baseUrl}/games`);
  }

  getGame(id: number): Observable<GameDetails> {
    return this.http.get<GameDetails>(`${this.baseUrl}/games/${id}`);
  }

  createGame(game: CreateGameDto): Observable<GameDetails> {
    return this.http.post<GameDetails>(`${this.baseUrl}/games`, game);
  }

  updateGame(id: number, game: CreateGameDto): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/games/${id}`, game);
  }

  deleteGame(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/games/${id}`);
  }

  getGenres(): Observable<Genre[]> {
    return this.http.get<Genre[]>(`${this.baseUrl}/genres`);
  }
}
