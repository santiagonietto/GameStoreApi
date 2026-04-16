import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { GamesService } from './games.service';
import { GameSummary } from './game.model';

@Component({
  selector: 'app-games-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './games-list.component.html',
  styleUrls: ['./games-list.component.css']
})
export class GamesListComponent implements OnInit {
  games: GameSummary[] = [];
  loading = true;
  error: string | null = null;
  deletingId: number | null = null;

  constructor(
    private gamesService: GamesService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadGames();
  }

  loadGames(): void {
    this.loading = true;
    this.error = null;
    console.log('Loading games...');
    this.gamesService.getGames().subscribe({
      next: (games) => {
        console.log('Games received:', games);
        this.games = games;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.log('Error loading games:', err);
        this.error = 'No se pudo conectar con la API. Verificá que el backend esté corriendo en http://localhost:5122';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  navigateToCreate(): void {
    this.router.navigate(['/games/new']);
  }

  navigateToEdit(id: number): void {
    this.router.navigate(['/games/edit', id]);
  }

  deleteGame(id: number, event: Event): void {
    event.stopPropagation();
    if (!confirm('¿Seguro que querés eliminar este juego?')) return;

    this.deletingId = id;
    this.gamesService.deleteGame(id).subscribe({
      next: () => {
        this.games = this.games.filter(g => g.id !== id);
        this.deletingId = null;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Error al eliminar el juego.';
        this.deletingId = null;
        this.cdr.detectChanges();
      }
    });
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(price);
  }

  formatDate(date: string): string {
    if (!date) return '-';
    const [year, month, day] = date.split('-');
    return `${day}/${month}/${year}`;
  }
}
