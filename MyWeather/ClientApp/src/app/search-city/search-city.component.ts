import { Component, ElementRef, EventEmitter, HostListener, OnInit, Output, ViewChild } from '@angular/core';

import { City } from '../_models/city.model';
import { CityService } from '../_services/city.service';

@Component({
  selector: 'app-search-city',
  templateUrl: './search-city.component.html',
  styleUrls: ['./search-city.component.css']
})
export class SearchCityComponent implements OnInit {
@Output() onSearch = new EventEmitter<City>();

  cities: City[] = [];
  selectedCity: City = null;
  showPopup = false;

  @ViewChild('s') inputText: ElementRef;

  constructor(
    private cityService: CityService,
    private eRef: ElementRef
  ) { }

  ngOnInit() {
  }

  onSearchCity() {
    this.selectedCity = null;
    const term: string = this.inputText.nativeElement.value;
    if (!term || term.length <= 2) {
      this.showPopup = false;
      return;
    }

    this.cityService.searchCity(term).subscribe(res => {
      // console.log(res);
      this.cities = res;
      this.showPopup = this.cities.length > 0;
    }, err => console.log);
  }

  onSelectCity(city: City) {
    this.selectedCity = city;
    this.inputText.nativeElement.value = city.name + ", " + city.country;
    this.showPopup = false;
  }

  onSearchClick(){
    if(!this.selectedCity) return;
    this.onSearch.emit(this.selectedCity);
  }

  @HostListener('document:click', ['$event'])
  onClick(event: Event) {
    if (!this.eRef.nativeElement.contains(event.target)) {
      this.showPopup = false;
    }
  }

}
