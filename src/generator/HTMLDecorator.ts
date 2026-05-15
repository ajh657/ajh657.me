import * as path from '@std/path';
import * as fs from '@std/fs';
import { DOMParser, Element, HTMLDocument, Node } from '@b-fuze/deno-dom';

export function DecorateHTML(websiteFolder: string, htmlData: string): string {
  let HTMLDocument = new DOMParser().parseFromString(htmlData, 'text/html');

  HTMLDocument = AddCSSRefs(HTMLDocument);

  const html = HTMLDocument.querySelector('html');

  if (html == undefined) {
    throw new Error('');
  }

  return html.outerHTML;
}

function AddCSSRefs(Document: HTMLDocument): HTMLDocument {
  Document.head.append();

  return Document;
}
