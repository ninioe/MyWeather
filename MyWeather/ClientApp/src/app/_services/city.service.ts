import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

import { City } from '../_models/city.model';
import { Weather } from '../_models/weather.model';

@Injectable()
export class CityService {
    baseUrl = environment.WebApiUri;

    constructor(private http: HttpClient) { }

    searchCity(term: string) {
        return this.http.get<City[]>(this.baseUrl + 'City/Search/?term=' + encodeURIComponent(term));
    }

    getCurrentWeather(city: City){
        return this.http.post<Weather>(this.baseUrl + 'City/GetCurrentWeather', city);
    }
    
}
