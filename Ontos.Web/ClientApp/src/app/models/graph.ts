

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

export class Relation {
  constructor(
    public id: string,
    public type: string,
    public originId: string,
    public targetId: string) {
  }
}

export class RelatedPage {
  constructor(
    public id: string,
    public type: string,
    public originId: string,
    public target: Page) {
  }
}

export class RelationType {
  constructor(
    public label: string,
    public directed: boolean,
    public acyclic: boolean) {
  }
}

export class NewPage {
  constructor(
    public content: string,
    public expression: NewExpression = null) {
  }
}

export class PageSearch {
  constructor(
    public language: string,
    public text: string) {
  }
}

export class PageSearchResult {
  constructor(
    public pageId: string,
    public score: number,
    public expressions: string[]) {
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

export class NewRelation {
  constructor(
    public type: string,
    public originId: string,
    public targetId: string) {
  }
}

export class UpdatePage {
  constructor(
    public id: number,
    public content: string) {
  }
}
