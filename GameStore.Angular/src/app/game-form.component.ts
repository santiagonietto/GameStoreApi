import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { GamesService } from './games.service';
import { Genre } from './game.model';

@Component({
  selector: 'app-game-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './game-form.component.html',
  styleUrls: ['./game-form.component.css']
})
export class GameFormComponent implements OnInit {
  form!: FormGroup;
  genres: Genre[] = [];
  isEditMode = false;
  gameId: number | null = null;
  loadingGenres = true;
  loadingGame = false;
  submitting = false;
  error: string | null = null;
  successMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private gamesService: GamesService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.buildForm();
    this.loadGenres();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.gameId = +id;
      this.loadGame(this.gameId);
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      genreId: ['', Validators.required],
      price: [null, [Validators.required, Validators.min(1), Validators.max(100)]],
      releaseDate: ['', Validators.required]
    });
  }

  loadGenres(): void {
    this.gamesService.getGenres().subscribe({
      next: (genres) => {
        this.genres = genres;
        this.loadingGenres = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'No se pudieron cargar los géneros.';
        this.loadingGenres = false;
        this.cdr.detectChanges();
      }
    });
  }

  loadGame(id: number): void {
    this.loadingGame = true;
    this.gamesService.getGame(id).subscribe({
      next: (game) => {
        this.form.patchValue({
          name: game.name,
          genreId: game.genreId,
          price: game.price,
          releaseDate: game.releaseDate
        });
        this.loadingGame = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'No se pudo cargar el juego.';
        this.loadingGame = false;
        this.cdr.detectChanges();
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid || this.submitting) return;
    this.submitting = true;
    this.error = null;

    const payload = {
      name: this.form.value.name.trim(),
      genreId: +this.form.value.genreId,
      price: +this.form.value.price,
      releaseDate: this.form.value.releaseDate
    };

    if (this.isEditMode && this.gameId) {
      this.gamesService.updateGame(this.gameId, payload).subscribe({
        next: () => {
          this.successMessage = 'Juego actualizado correctamente.';
          this.submitting = false;
          setTimeout(() => this.router.navigate(['/games']), 1200);
        },
        error: () => {
          this.error = 'Error al actualizar el juego.';
          this.submitting = false;
        }
      });
    } else {
      this.gamesService.createGame(payload).subscribe({
        next: () => {
          this.successMessage = 'Juego creado correctamente.';
          this.submitting = false;
          setTimeout(() => this.router.navigate(['/games']), 1200);
        },
        error: () => {
          this.error = 'Error al crear el juego.';
          this.submitting = false;
        }
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/games']);
  }

  getFieldError(field: string): string | null {
    const control = this.form.get(field);
    if (!control || !control.invalid || !control.touched) return null;

    if (control.errors?.['required']) return 'Este campo es requerido.';
    if (control.errors?.['minlength']) return `Mínimo ${control.errors['minlength'].requiredLength} caracteres.`;
    if (control.errors?.['maxlength']) return `Máximo ${control.errors['maxlength'].requiredLength} caracteres.`;
    if (control.errors?.['min']) return `El valor mínimo es ${control.errors['min'].min}.`;
    if (control.errors?.['max']) return `El valor máximo es ${control.errors['max'].max}.`;

    return null;
  }
}
