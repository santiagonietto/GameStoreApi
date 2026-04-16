import { Routes } from '@angular/router';
import { GamesListComponent } from './games-list.component';
import { GameFormComponent } from './game-form.component';

export const routes: Routes = [
  { path: '', redirectTo: '/games', pathMatch: 'full' },
  { path: 'games', component: GamesListComponent },
  { path: 'games/new', component: GameFormComponent },
  { path: 'games/edit/:id', component: GameFormComponent },
  { path: '**', redirectTo: '/games' }
];
