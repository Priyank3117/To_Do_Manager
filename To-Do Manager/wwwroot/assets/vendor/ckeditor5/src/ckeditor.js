"use strict";
///**
// * @license Copyright (c) 2014-2024, CKSource Holding sp. z o.o. All rights reserved.
// * For licensing, see LICENSE.md or https://ckeditor.com/legal/ckeditor-oss-license
// */
//import { ClassicEditor } from '@ckeditor/ckeditor5-editor-classic';
//import { AIAssistant } from '@ckeditor/ckeditor5-ai';
//import { Alignment } from '@ckeditor/ckeditor5-alignment';
//import { Autoformat } from '@ckeditor/ckeditor5-autoformat';
//import { Autosave } from '@ckeditor/ckeditor5-autosave';
//import { Bold, Code, Italic } from '@ckeditor/ckeditor5-basic-styles';
//import { BlockQuote } from '@ckeditor/ckeditor5-block-quote';
//import { CKBox, CKBoxImageEdit } from '@ckeditor/ckeditor5-ckbox';
//import { CloudServices } from '@ckeditor/ckeditor5-cloud-services';
//import { CodeBlock } from '@ckeditor/ckeditor5-code-block';
//import { Comments } from '@ckeditor/ckeditor5-comments';
//import type { EditorConfig } from '@ckeditor/ckeditor5-core';
//import { DocumentOutline } from '@ckeditor/ckeditor5-document-outline';
//import { Essentials } from '@ckeditor/ckeditor5-essentials';
//import { ExportPdf } from '@ckeditor/ckeditor5-export-pdf';
//import { ExportWord } from '@ckeditor/ckeditor5-export-word';
//import { FindAndReplace } from '@ckeditor/ckeditor5-find-and-replace';
//import { FontBackgroundColor, FontColor, FontFamily, FontSize } from '@ckeditor/ckeditor5-font';
//import { Heading } from '@ckeditor/ckeditor5-heading';
//import { HorizontalLine } from '@ckeditor/ckeditor5-horizontal-line';
//import { HtmlEmbed } from '@ckeditor/ckeditor5-html-embed';
//import { DataFilter, DataSchema } from '@ckeditor/ckeditor5-html-support';
//import {
//	AutoImage,
//	Image,
//	ImageCaption,
//	ImageInsert,
//	ImageResize,
//	ImageStyle,
//	ImageToolbar,
//	ImageUpload,
//	PictureEditing
//} from '@ckeditor/ckeditor5-image';
//import { ImportWord } from '@ckeditor/ckeditor5-import-word';
//import { Indent, IndentBlock } from '@ckeditor/ckeditor5-indent';
//import { AutoLink, Link } from '@ckeditor/ckeditor5-link';
//import { List, ListProperties } from '@ckeditor/ckeditor5-list';
//import { MultiLevelList } from '@ckeditor/ckeditor5-list-multi-level';
//import { Markdown } from '@ckeditor/ckeditor5-markdown-gfm';
//import { MediaEmbed, MediaEmbedToolbar } from '@ckeditor/ckeditor5-media-embed';
//import { Mention } from '@ckeditor/ckeditor5-mention';
//import { Paragraph } from '@ckeditor/ckeditor5-paragraph';
//import { PasteFromOffice } from '@ckeditor/ckeditor5-paste-from-office';
//import { Table, TableToolbar } from '@ckeditor/ckeditor5-table';
//import { TextTransformation } from '@ckeditor/ckeditor5-typing';
//import { AccessibilityHelp } from '@ckeditor/ckeditor5-ui';
//import { Undo } from '@ckeditor/ckeditor5-undo';
//// You can read more about extending the build with additional plugins in the "Installing plugins" guide.
//// See https://ckeditor.com/docs/ckeditor5/latest/installation/plugins/installing-plugins.html for details.
//class Editor extends ClassicEditor {
//	public static override builtinPlugins = [
//		AIAssistant,
//		AccessibilityHelp,
//		Alignment,
//		AutoImage,
//		AutoLink,
//		Autoformat,
//		Autosave,
//		BlockQuote,
//		Bold,
//		CKBox,
//		CKBoxImageEdit,
//		CloudServices,
//		Code,
//		CodeBlock,
//		Comments,
//		DataFilter,
//		DataSchema,
//		DocumentOutline,
//		Essentials,
//		ExportPdf,
//		ExportWord,
//		FindAndReplace,
//		FontBackgroundColor,
//		FontColor,
//		FontFamily,
//		FontSize,
//		Heading,
//		HorizontalLine,
//		HtmlEmbed,
//		Image,
//		ImageCaption,
//		ImageInsert,
//		ImageResize,
//		ImageStyle,
//		ImageToolbar,
//		ImageUpload,
//		ImportWord,
//		Indent,
//		IndentBlock,
//		Italic,
//		Link,
//		List,
//		ListProperties,
//		Markdown,
//		MediaEmbed,
//		MediaEmbedToolbar,
//		Mention,
//		MultiLevelList,
//		Paragraph,
//		PasteFromOffice,
//		PictureEditing,
//		Table,
//		TableToolbar,
//		TextTransformation,
//		Undo
//	];
//	public static override defaultConfig: EditorConfig = {
//		toolbar: {
//			items: [
//				'heading',
//				'|',
//				'bold',
//				'italic',
//				'link',
//				'bulletedList',
//				'numberedList',
//				'|',
//				'outdent',
//				'indent',
//				'|',
//				'imageUpload',
//				'blockQuote',
//				'insertTable',
//				'mediaEmbed',
//				'undo',
//				'redo',
//				'ckboxImageEdit',
//				'codeBlock',
//				'comment',
//				'exportWord',
//				'fontSize',
//				'findAndReplace',
//				'exportPdf'
//			]
//		},
//		language: 'en',
//		image: {
//			toolbar: [
//				'comment',
//				'imageTextAlternative',
//				'toggleImageCaption',
//				'imageStyle:inline',
//				'imageStyle:block',
//				'imageStyle:side'
//			]
//		},
//		table: {
//			contentToolbar: [
//				'tableColumn',
//				'tableRow',
//				'mergeTableCells'
//			],
//			tableToolbar: [
//				'comment'
//			]
//		},
//		comments: {
//			editorConfig: {
//				extraPlugins: [
//					Autoformat,
//					Bold,
//					Italic,
//					List
//				]
//			}
//		}
//	};
//}
//export default Editor;
//# sourceMappingURL=ckeditor.js.map