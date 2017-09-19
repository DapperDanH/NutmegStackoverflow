import { Component, ElementRef, AfterViewInit, OnInit } from '@angular/core';
import { Router, NavigationStart, NavigationEnd, NavigationCancel, NavigationError } from '@angular/router';
import { Response } from '@angular/http';
import { NavMenuComponent } from './navmenu/navmenu.component';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
	constructor(private router: Router) {
	}

	ngOnInit() {
		var preLoadDiv = document.getElementById("pre-load");
		if (preLoadDiv) {
			preLoadDiv.parentNode.removeChild(preLoadDiv);
		}

		//var seeding = document.getElementById("seeding");
		//var seed: ApplicationSeed = JSON.parse(seeding.innerHTML);

		//this.services.authentication.displayName = seed.displayName;
		//this.services.authentication.roles = seed.roles;

		this.listenToRouteChanges();
	}

	private listenToRouteChanges(){
		var loadId: number;
		this.router.events.forEach((event) => {
			if (event instanceof NavigationStart) {
				//this.services.loading.reset();
				//loadId = this.services.loading.begin();
			}
			else if (event instanceof NavigationEnd || event instanceof NavigationCancel) {
				//this.services.loading.end(loadId);
			}
			else if (event instanceof NavigationError) {
				//this.services.loading.reset();
				//this.services.notification.error("Failed to navigate");
			}
		});
	}
}
