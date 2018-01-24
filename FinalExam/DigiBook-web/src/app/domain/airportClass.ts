import {Airport} from '../domain/airport';
export class AirportClass implements Airport{
    constructor(
        public code?,
        public name?,
    ){}
}
