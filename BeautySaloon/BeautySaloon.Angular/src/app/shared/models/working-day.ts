export interface WorkingDay {
    workingDayId: number | null;
    scheduleId: number | null;
    day: string;
    startTime: string; // Изменим на строку для представления времени
    endTime: string; // Изменим на строку для представления времени
  }