import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppInitService } from './core/services/app-init.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  standalone: true,
  imports: [RouterOutlet],
})
export class AppComponent implements OnInit {

  constructor(private appInitService: AppInitService) {}

  ngOnInit() {
    // Initialize app data from localStorage
    this.appInitService.initializeApp();
  }
}
