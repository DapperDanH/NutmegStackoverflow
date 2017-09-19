import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
	styleUrls: ['./navmenu.component.css'],
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavMenuComponent {
	constructor(public router: Router) {
	}
}
