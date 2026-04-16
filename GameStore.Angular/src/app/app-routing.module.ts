import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GamesListComponent } from './games-list.component';
import { GameFormComponent } from './game-form.component';

const routes: Routes = [
  { path: '', redirectTo: '/games', pathMatch: 'full' },
  { path: 'games', component: GamesListComponent },
  { path: 'games/new', component: GameFormComponent },
  { path: 'games/edit/:id', component: GameFormComponent },
  { path: '**', redirectTo: '/games' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
