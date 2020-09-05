

export class Page {
  constructor(
    public id: number,
    public content: string,
    public references: Reference[] = []) {
  }
}

export class Reference {
  constructor(
    public id: number,
    public pageId: number,
    public expression: Expression) {
  }
}

export class Expression {
  constructor(
    public id: number,
    public language: string,
    public label: string) {
  }
}

export class NewPage {
  constructor(
    public content: string,
    public expression: NewExpression = null) {
  }
}

export class NewExpression {
  constructor(
    public language: string,
    public label: string) {
  }
}

export class NewReference {
  constructor(
    public pageId: number,
    public expression: NewExpression) {
  }
}

export class UpdatePage {
  constructor(
    public id: number,
    public content: string) {
  }
}
