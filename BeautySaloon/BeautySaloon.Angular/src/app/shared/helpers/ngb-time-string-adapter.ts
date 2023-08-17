import { Injectable } from "@angular/core";
import { NgbTimeAdapter, NgbTimeStruct } from "@ng-bootstrap/ng-bootstrap";

const pad = (i: number): string => (i < 10 ? `0${i}` : `${i}`);

/**
 * Example of a String Time adapter
 */
@Injectable()
export class NgbTimeStringAdapter extends NgbTimeAdapter<string> {
  fromModel(value: string | null): NgbTimeStruct | null {
    if (!value) {
      return null;
    }
    const split = value.split(':');
    var result = {
      hour: parseInt(split[0], 10),
      minute: parseInt(split[1], 10),
      second: parseInt(split[2], 10),
    }

    if (!result.hour) {
      result.hour = 0;
    }

    if (!result.minute) {
      result.minute = 0;
    }
    if (!result.second) {
      result.second = 0;
    }

    return result;
  }

  toModel(time: NgbTimeStruct | null): string | null {
    if (!time){
      return null;
    }
    if (!time.hour) {
      time.hour = 0;
    }

    if (!time.minute) {
      time.minute = 0;
    }
    if (!time.second) {
      time.second = 0;
    }
    return time != null ? `${pad(time.hour)}:${pad(time.minute)}:${pad(time.second)}` : null;
  }
}