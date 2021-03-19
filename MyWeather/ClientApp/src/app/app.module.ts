import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { CityService } from './_services/city.service';
import { SearchCityComponent } from './search-city/search-city.component';
import { FavoriteService } from './_services/favorite.service';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    SearchCityComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' }
    ])
  ],
  providers: [CityService, FavoriteService],
  bootstrap: [AppComponent]
})
export class AppModule { }
