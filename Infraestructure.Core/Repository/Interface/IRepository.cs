namespace Infraestructure.Core.Repository.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRepository<TEntity> where TEntity : class
    {
        #region IRepository<T> Members

        /// Retorna un objeto del tipo AsQueryable
        IQueryable<TEntity> AsQueryable();

        /// Retorna un objeto del tipo AsQueryable y acepta como parámetro las relaciones a incluir
        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity Find(Expression<Func<TEntity, bool>> predicate);

        /// Función que retorna una entidad, a partir de una consulta.
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// Función que retorna una entidad, a partir de una consulta sin atachar.
        IEnumerable<TEntity> FindAllNoTracking(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// Retorna la ultima entidad encontrada bajo una condición especificada
        TEntity First(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// Retorna la primera entidad encontrada bajo una condición especificada
        TEntity Last(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// Retorna la primera entidad encontrada bajo una condición especificada o null sino encontrara registros
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// Retorna la ultima entidad encontrada bajo una condición especificada o null sino encontrara registros
        TEntity LastOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// Retorna una entidad bajo una condición especificada
        TEntity Single(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// Retorna una entidad bajo una condición especificada o null sino encontrara registros
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// Registra una entidad
        void Insert(TEntity entity);

        /// Registra varias entidades
        void Insert(IEnumerable<TEntity> entities);

        /// Actualiza una entidad
        void Update(TEntity entity);

        /// Actualiza varias entidades
        void Update(IEnumerable<TEntity> entities);

        /// Elimina una entidad
        void Delete(TEntity entity);

        /// Elimina por Id
        void Delete(object id);

        void Delete(IEnumerable<int> id);

        /// Elimina un conjuto de entidades
        void Delete(IEnumerable<TEntity> entities);

        #endregion IRepository<T> Members

        #region SQL Queries

        /// <summary>
        /// Ejecuta sentencias SQL con parametros
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string query, params object[] parameters);

        #endregion SQL Queries
    }
}