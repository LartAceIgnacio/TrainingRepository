import { Pilot } from '../pilot/pilot';
export class PilotClass implements Pilot {
    constructor(
           public pilotId?  ,
           public firstName? ,
           public middleName ?,
           public lastName? ,
           public yearsOfExperience?,
           public pilotCode? ,
           public dateCreated? ,
           public dateModified? ,
           public dateActivated?,
           public dateOfBirth?
    ) { }
}