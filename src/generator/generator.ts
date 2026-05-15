import * as path from '@std/path';
import * as fs from '@std/fs';
import { DecorateHTML } from './HTMLDecorator.ts';

export function Generate(websiteFolder: string, outputFolder: string) {
  const websiteBaseFolder = path.resolve(websiteFolder, 'base');

  iterateBaseFolder(websiteFolder, [], outputFolder);
}

function iterateBaseFolder(
  rootFolder: string,
  currentFolderTree: string[],
  outputFolder: string,
) {
  const websiteBaseFolder = path.resolve(rootFolder, 'base');
  for (const entry of Deno.readDirSync(
    path.resolve(websiteBaseFolder, ...currentFolderTree),
  )) {
    if (entry.isDirectory) {
      const newFolder = path.resolve(outputFolder, entry.name);

      if (!fs.existsSync(newFolder)) {
        Deno.mkdirSync(newFolder);
      }

      iterateBaseFolder(
        rootFolder,
        [...currentFolderTree, entry.name],
        outputFolder,
      );
    } else if (entry.isFile) {
      if (entry.name.endsWith('.html')) {
        let htmlData = Deno.readTextFileSync(
          path.resolve(websiteBaseFolder, ...currentFolderTree, entry.name),
        ).toString();

        htmlData = DecorateHTML(rootFolder, htmlData);

        Deno.writeTextFileSync(
          path.resolve(outputFolder, ...currentFolderTree, entry.name),
          htmlData,
        );
      } else {
        Deno.copyFileSync(
          path.resolve(rootFolder, ...currentFolderTree, entry.name),
          path.resolve(outputFolder, ...currentFolderTree, entry.name),
        );
      }
    }
  }
}
