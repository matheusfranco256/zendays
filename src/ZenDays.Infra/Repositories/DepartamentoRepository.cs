﻿using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ZenDays.Domain.Entities;
using ZenDays.Infra.Interfaces;

namespace ZenDays.Infra.Repositories
{
    public class DepartamentoRepository : BaseRepository<Departamento>, IDepartamentoRepository
    {
        private IHostingEnvironment _environment;

        public DepartamentoRepository(IConfiguration configuration, IHostingEnvironment environment) : base(configuration, environment)
        {
            _environment = environment;
        }

        public async Task<Departamento?> GetByName(string name, bool isEnabled = true)
        {
            DocumentSnapshot? documentSnapshot = null!;
            Query query = _fireStoreDb.Collection(typeof(Departamento).Name)
                .WhereEqualTo("Nome", name)
                .WhereEqualTo("Ativo", isEnabled);
            QuerySnapshot? snapshot = await query.GetSnapshotAsync();
            documentSnapshot = snapshot.Documents.FirstOrDefault();

            if (documentSnapshot != null)
            {
                Dictionary<string, object> entity = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(entity);
                var newEntity = JsonConvert.DeserializeObject<Departamento>(json);
                if (newEntity == null) return default;
                newEntity.Id = documentSnapshot.Id;
                return newEntity;
            }
            return default;
        }
    }
}
