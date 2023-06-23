﻿using Google.Cloud.Firestore;
using Newtonsoft.Json;
using ZenDays.Domain.Entities;
using ZenDays.Infra.Interfaces;

namespace ZenDays.Infra.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : Base
    {
        protected string diretorio = "C:\\Users\\Matheus\\Source\\ZenDays\\zendays.json";
        protected string projectId;
        protected FirestoreDb _fireStoreDb;

        public BaseRepository()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", diretorio);
            projectId = "zendays-81a95";
            _fireStoreDb = FirestoreDb.Create(projectId);
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
    }

}

