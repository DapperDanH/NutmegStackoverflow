import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { PlayerComponent } from './player.component';

@NgModule({
    declarations: [
		PlayerComponent,
    ],
	imports: [

		RouterModule.forChild([{
			path: 'player',
			component: PlayerComponent,
		}])
	],
	entryComponents: [
	]
})
export class PlayerModule {
}
