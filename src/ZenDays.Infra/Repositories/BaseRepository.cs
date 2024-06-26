﻿using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ZenDays.Domain.Entities;
using ZenDays.Infra.Interfaces;

namespace ZenDays.Infra.Repositories
{
	public abstract class BaseRepository<T> : IBaseRepository<T> where T : Base
	{
		protected FirestoreDb _fireStoreDb;
		private readonly IConfiguration _configuration;


		public BaseRepository(IConfiguration configuration, IHostingEnvironment environment)
		{
			_configuration = configuration;
			_fireStoreDb = CreateFirestoreDb(Path.Combine(environment.WebRootPath, $"{"zendays"}.json"));
		}

		private FirestoreDb CreateFirestoreDb(string path)
		{
			string fullPath = "";
			string? jsonFilePath = path;
			string? projectId = _configuration.GetSection("FirestoreConfig:ProjectId").Value;
			if (jsonFilePath != null) fullPath = Path.GetFullPath(jsonFilePath);

			Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", fullPath);

			return FirestoreDb.Create(projectId);
		}

		public async Task<T?> Create(Dictionary<string, object> obj)
		{
			var addedDocument = await _fireStoreDb.Collection(typeof(T).Name).AddAsync(obj);
			var documentSnapshot = await addedDocument.GetSnapshotAsync();
			var entity = JsonConvert.DeserializeObject<T?>(JsonConvert.SerializeObject(obj));
			if (entity == null) return default;
			entity.Id = documentSnapshot.Id;
			return entity;
		}

		public async Task<List<T>> GetAll()
		{
			Query query = _fireStoreDb.Collection(typeof(T).Name)
				  .WhereEqualTo("Ativo", true);

			QuerySnapshot QuerySnapshot = await query.GetSnapshotAsync();
			List<T> entitys = new();

			foreach (DocumentSnapshot documentSnapshot in QuerySnapshot.Documents)
			{
				if (documentSnapshot.Exists)
				{
					Dictionary<string, object> entity = documentSnapshot.ToDictionary();
					string json = JsonConvert.SerializeObject(entity);
					var newEntity = JsonConvert.DeserializeObject<T>(json);
					if (newEntity != null)
					{
						newEntity.Id = documentSnapshot.Id;
						entitys.Add(newEntity);
					}
				}
			}

			return entitys;
		}

		public async Task<List<T>> GetAll(List<(string field, string value)> filtros)
		{
			Query query = _fireStoreDb.Collection(typeof(T).Name)
				  .WhereEqualTo("Ativo", true);

			foreach (var f in filtros)
			{
				query = query.WhereEqualTo(f.field, f.value);
			}


			QuerySnapshot QuerySnapshot = await query.GetSnapshotAsync();
			List<T> entitys = new();

			foreach (DocumentSnapshot documentSnapshot in QuerySnapshot.Documents)
			{
				if (documentSnapshot.Exists)
				{
					Dictionary<string, object> entity = documentSnapshot.ToDictionary();
					string json = JsonConvert.SerializeObject(entity);
					var newEntity = JsonConvert.DeserializeObject<T>(json);
					if (newEntity != null)
					{
						newEntity.Id = documentSnapshot.Id;
						entitys.Add(newEntity);
					}
				}
			}

			return entitys;
		}

		public async Task<T?> Get(string id, bool isEnabled = true)
		{
			DocumentSnapshot? documentSnapshot = null!;
			Query query = _fireStoreDb.Collection(typeof(T).Name)
				.WhereEqualTo(FieldPath.DocumentId, id)
				.WhereEqualTo("Ativo", true);
			QuerySnapshot? snapshot = await query.GetSnapshotAsync();
			documentSnapshot = snapshot.Documents.FirstOrDefault();

			if (documentSnapshot != null)
			{
				Dictionary<string, object> entity = documentSnapshot.ToDictionary();
				string json = JsonConvert.SerializeObject(entity);
				var newEntity = JsonConvert.DeserializeObject<T>(json);
				if (newEntity == null) return default;
				newEntity.Id = documentSnapshot.Id;
				return newEntity;
			}
			return default;
		}

		public async Task<T?> Update(Dictionary<string, object> obj, string id)
		{
			DocumentSnapshot? documentSnapshot = null!;
			Query query = _fireStoreDb.Collection(typeof(T).Name)
				.WhereEqualTo(FieldPath.DocumentId, id)
				.WhereEqualTo("Ativo", true);
			QuerySnapshot? snapshot = await query.GetSnapshotAsync();
			documentSnapshot = snapshot.Documents.FirstOrDefault();

			if (documentSnapshot != null)
			{
				await documentSnapshot.Reference.SetAsync(obj, SetOptions.Overwrite);
				var entity = JsonConvert.DeserializeObject<T?>(JsonConvert.SerializeObject(obj));
				if (entity == null) return default;
				entity.Id = documentSnapshot.Id;
				return entity;
			}
			return default;

		}

		public async Task Disable(Dictionary<string, object> obj, string id)
		{
			DocumentSnapshot? documentSnapshot = null!;
			Query query = _fireStoreDb.Collection(typeof(T).Name)
				.WhereEqualTo(FieldPath.DocumentId, id)
				.WhereEqualTo("Ativo", true);
			QuerySnapshot? snapshot = await query.GetSnapshotAsync();
			documentSnapshot = snapshot.Documents.FirstOrDefault();

			if (documentSnapshot != null)
			{
				await documentSnapshot.Reference.SetAsync(obj, SetOptions.Overwrite);
				var entity = JsonConvert.DeserializeObject<T?>(JsonConvert.SerializeObject(obj));
				if (entity == null) return;
				entity.Id = documentSnapshot.Id;
				return;
			}
			return;
		}

		public async Task Delete(Dictionary<string, object> obj, string id)
		{
			DocumentSnapshot? documentSnapshot = null!;
			Query query = _fireStoreDb.Collection(typeof(T).Name)
				.WhereEqualTo(FieldPath.DocumentId, id);
			QuerySnapshot? snapshot = await query.GetSnapshotAsync();
			documentSnapshot = snapshot.Documents.FirstOrDefault();

			if (documentSnapshot != null)
			{
				await documentSnapshot.Reference.DeleteAsync();
				var entity = JsonConvert.DeserializeObject<T?>(JsonConvert.SerializeObject(obj));
				if (entity == null) return;
				entity.Id = documentSnapshot.Id;
				return;
			}
			return;
		}

		public async Task DeleteFromFirebaseAuth(string uid)
		{
			// Initialize Firebase Admin SDK
			GoogleCredential googleCredential = GoogleCredential.GetApplicationDefault();

			FirestoreClientBuilder builder = new FirestoreClientBuilder
			{
				ChannelCredentials = googleCredential.ToChannelCredentials()
			};

			// Replace with the user's UID you want to delete


			// Remove the user from Firebase Authentication
			try
			{
				await FirebaseAuth.DefaultInstance.DeleteUserAsync(uid);
			}
			catch (FirebaseAuthException e)
			{
				Console.WriteLine($"Error deleting user: {e.Message}");
			}
		}
	}

}

