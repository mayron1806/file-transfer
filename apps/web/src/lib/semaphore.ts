export class Semaphore {
  private tasks: (() => void)[] = [];
  private available: number;

  constructor(private count: number) {
    this.available = count;
  }

  async acquire(): Promise<void> {
    if (this.available > 0) {
      this.available--;
      return;
    }
    return new Promise(resolve => this.tasks.push(resolve));
  }

  release(): void {
    if (this.tasks.length > 0) {
      const nextTask = this.tasks.shift();
      if (nextTask) nextTask();
    } else {
      this.available++;
    }
  }
}