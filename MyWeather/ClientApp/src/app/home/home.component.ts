import { Component, OnInit } from '@angular/core';

import { City } from '../_models/city.model';
import { Weather } from '../_models/weather.model';
import { CityService } from '../_services/city.service';
import { FavoriteService } from '../_services/favorite.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  currentWeather: Weather = null;
  favCities: City[] = [];

  constructor(
    private cityService: CityService,
    private favService: FavoriteService
  ) { }

  ngOnInit() {
    this.getFavoriteCities();
  }

  getFavoriteCities(){
    this.favService.getFavoriteCities().subscribe(res => {
      this.favCities = res;
    });
  }

  toggleFav() {
    if (this.currentWeather.isFavorite) {
      this.favService.deleteFavorite(this.currentWeather.key).subscribe(res => {
        this.getFavoriteCities();
      });
    } else {
      this.favService.addFavorite(this.currentWeather.key).subscribe(res => {
        this.getFavoriteCities();
      });
    }
    this.currentWeather.isFavorite = !this.currentWeather.isFavorite;
  }

  onSearchCity(city: City) {
    this.cityService.getCurrentWeather(city).subscribe((res) => {
      //console.log(res);
      this.currentWeather = res;
    });
  }
}
