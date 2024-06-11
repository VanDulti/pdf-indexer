# PdfIndexer

[![.NET](https://github.com/VanDulti/pdf-indexer/actions/workflows/dotnet.yml/badge.svg)](https://github.com/VanDulti/pdf-indexer/actions/workflows/dotnet.yml)
<!--[![Lint Code Base](https://github.com/VanDulti/pdf-indexer/actions/workflows/super-linter.yml/badge.svg)](https://github.com/VanDulti/pdf-indexer/actions/workflows/super-linter.yml)-->
[![Publish & Deploy to Github Pages](https://github.com/VanDulti/pdf-indexer/actions/workflows/pages.yml/badge.svg)](https://github.com/VanDulti/pdf-indexer/actions/workflows/pages.yml)

## Introduction

This is a simple tool to generate word indices for PDF files.
The indices can then be displayed in a table and downloaded in various ways & formats.

I created this project 1. because I needed a tool like this and 2. as a project for the
course [Software Development with C#](https://ssw.jku.at/Teaching/Lectures/CSharp/) at JKU.

## What is a word index?

A word index is a list of words that appear in a document, along with the page numbers on which they appear.
It is used to quickly find where a word appears in a document.

See [Wikipedia]("https://de.wikipedia.org/wiki/Register_(Nachschlagewerk)#Konkordanzen")

## Why?

Sometimes, Ctrl+F is not available. Think of a document you want to read on paper, this project resembles a way to
quickly find where a word appears in that document.

## Tech Stack

- .NET 8
- Blazor WebAssembly
- XUnit
- Bootstrap
- Water.css
- Hosted on GitHub Pages (CI/CD via GitHub Actions)