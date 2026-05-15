import * as path from '@std/path';
import * as fs from '@std/fs';
import { Generate } from './generator.ts';

function resolveCWD(): string {
  let cwd = Deno.cwd();
  if (
    Deno.readDirSync(cwd)
      .toArray()
      .find((x) => x.name == '.git')
  ) {
    cwd = path.resolve(cwd, 'src', 'generator');
  }
  return cwd;
}

const workDir = resolveCWD();

const websiteFolder = path.resolve(workDir, '..', 'website');

console.log(websiteFolder);

const outputFolderPath = path.resolve(workDir, 'output');

if (!fs.existsSync(outputFolderPath)) {
  Deno.mkdirSync(outputFolderPath);
}

Generate(websiteFolder, outputFolderPath);
