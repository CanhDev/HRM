import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./maincss.css'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent {
  title = 'my-angular-project';
  openSubMenu: string | null = null;

toggleSubMenu(menu: string) {
  this.openSubMenu = this.openSubMenu === menu ? null : menu;
}

}
