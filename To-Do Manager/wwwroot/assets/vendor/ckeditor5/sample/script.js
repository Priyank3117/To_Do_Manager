createDialog().then( config => {
	return ClassicEditor
		.create( document.querySelector( '.editor' ), {
			licenseKey: config.licenseKey,
			ckbox: {
				tokenUrl: config.ckboxTokenUrl
			},
			documentOutline: {
				container: document.querySelector( '.document-outline-container' )
			},
			sidebar: {
				container: document.querySelector( '.sidebar' )
			},
			extraPlugins: [
				// Learn more about users at https://ckeditor.com/docs/ckeditor5/latest/features/collaboration/users.html.
				class UsersInit {
					static get requires() {
						return [ 'Users' ];
					}

					constructor( editor ) {
						this.editor = editor;
					}

					init() {
						const users = this.editor.plugins.get( 'Users' );

						users.addUser( {
							id: 'u1'
						} );

						users.defineMe( 'u1' );
					}
				}
			]
		} )
		.then( editor => {
			window.editor = editor;
		} )
		.catch( handleSampleError );
} );

function handleSampleError( error ) {
	const issueUrl = 'https://github.com/ckeditor/ckeditor5/issues';

	const message = [
		'Oops, something went wrong!',
		`Please, report the following error on ${ issueUrl } with the build id "khiii4yisbbo-hdzn8bju79o5" and the error stack trace:`
	].join( '\n' );

	console.error( message );
	console.error( error );
}
