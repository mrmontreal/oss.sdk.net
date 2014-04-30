﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Silanis.ESL.SDK;
using Silanis.ESL.SDK.Builder;

namespace SDK.Examples
{
	public class DocumentOperationsExample : SDKSample
	{
        public DocumentPackage BuiltPackage{ get; set; }
        public DocumentPackage RetrievedPackage{ get; set; }
        public DocumentPackage RetrievedPackageWithNewDocument{ get; set; }
        public DocumentPackage RetrievedPackageWithUpdatedDocument{ get; set; }
        public DocumentPackage RetrievedPackageWithDeletedDocument{ get; set; }
        
        public const string OriginalDocumentName = "Original Document Name";
        public const string UpdatedDocumentName = "Original Document Description";
        
        public const string OriginalDocumentDescription = "Updated Document Name";
        public const string UpdatedDocumentDescription = "Updated Document Description";
    
		public DocumentOperationsExample(Props props) : base(props.Get("api.url"), props.Get("api.key"))
		{
		}

		override public void Execute()
		{
			FileInfo file = new FileInfo(Directory.GetCurrentDirectory() + "/src/document.pdf");

			// 1. Create a package
			BuiltPackage = PackageBuilder.NewPackageNamed("Policy " + DateTime.Now)
				.DescribedAs("This is a package demonstrating document upload")
			    .WithSigner(SignerBuilder.NewSignerWithEmail("john.smith@email.com")
      					.WithFirstName("John")
      					.WithLastName("Smith")
      					.WithTitle("Managing Director")
      					.WithCompany("Acme Inc."))
  				.Build();

            PackageId packageId = eslClient.CreatePackage(BuiltPackage);
			Console.WriteLine("package created, id = " + packageId);
            
            RetrievedPackage = eslClient.GetPackage(packageId);

			// 2. Construct a document
			Document document = DocumentBuilder.NewDocumentNamed( OriginalDocumentName )
                .WithDescription( OriginalDocumentDescription )
				.FromFile(file.FullName)
				.WithSignature(SignatureBuilder.SignatureFor("john.smith@email.com")
						.OnPage(0))                                
				.Build();

			// 3. Attach the document to the created package by uploading the document.
			document = eslClient.UploadDocument(document, RetrievedPackage);
			Console.WriteLine("Document was uploaded");
            
            RetrievedPackageWithNewDocument = eslClient.GetPackage(packageId);

			//This is how you would update a document's metadata (name, description)
			document.Name = UpdatedDocumentName;
			document.Description = UpdatedDocumentDescription;

			eslClient.PackageService.UpdateDocumentMetadata(RetrievedPackage, document);
			Console.WriteLine("Document was updated");

            RetrievedPackageWithUpdatedDocument = eslClient.GetPackage(packageId);

			//This is how you would delete a document from a package
			eslClient.PackageService.DeleteDocument(packageId, document.Id);

            RetrievedPackageWithDeletedDocument = eslClient.GetPackage(packageId);
		}

		public static void Main(string[] args)
		{
			new DocumentOperationsExample(Props.GetInstance()).Run();
		}
	}
}