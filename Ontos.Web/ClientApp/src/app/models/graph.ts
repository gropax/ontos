

export class Page {
  constructor(
    public id: number,
    public content: string) {
  }
}

export class NewPage {
  constructor(
    public content: string) {
  }
}

export class UpdatePage {
  constructor(
    public id: number,
    public content: string) {
  }
}
