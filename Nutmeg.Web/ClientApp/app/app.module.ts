import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { RouterModule, Router } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component'
import { NavMenuComponent } from './navmenu';
import { PlayerModule } from './player/player.module';


@NgModule({
    bootstrap: [ AppComponent ],
	declarations: [
        AppComponent,
		NavMenuComponent,
    ],
	imports: [
		BrowserModule,
		HttpModule,
		BrowserAnimationsModule,
		PlayerModule,
        RouterModule.forRoot([
			{ path: '', redirectTo: 'player', pathMatch: 'full' },
			{ path: '**', redirectTo: 'player' }
		])
	],
	entryComponents: [
	],
})
export class AppModule {
}
